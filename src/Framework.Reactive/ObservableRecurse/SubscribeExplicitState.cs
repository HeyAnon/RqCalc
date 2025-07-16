using System.ComponentModel;
using System.Reactive.Linq;

namespace Framework.Reactive.ObservableRecurse;

internal class SubscribeState<TSource> : IDisposable
    where TSource : class, INotifyPropertyChanged
{
    private readonly TSource source;
    private readonly Func<ObservableRule<TSource>, ILinkNode>[] subRules;

    private readonly List<IDisposable> items = [];

    private readonly bool lockEvents;
    private bool eventRaised;
        
    public SubscribeState (TSource source, Func<ObservableRule<TSource>, ILinkNode>[] subRules, bool lockEvents)
    {
        this.source = source;
        this.subRules = subRules;
        this.lockEvents = lockEvents;

        this.Subscribe ();
    }

    private void Subscribe ()
    {
        var rootNode = new Node<TSource> (from subRule in this.subRules
                                          select subRule (ObservableRule<TSource>.Value));


        var lastSubscribeActions = new List<Func<IDisposable>> ();

        this.InnerSubscribe(this.source, rootNode, lastSubscribeActions);

        lastSubscribeActions.ForEach (v => this.items.Add (v ()));
    }

    private void UnSubscribe ()
    {
        this.items.ForEach (v => v.Dispose ());
        this.items.Clear ();
    }

    private void RefreshSubscribe ()
    {
        this.UnSubscribe();
        this.Subscribe();
    }

    private void ProxyEvent (Action action)
    {
        if (action == null) throw new ArgumentNullException("action");

        if (this.lockEvents)
        {
            if (this.eventRaised)
            {
                return;
            }

            this.eventRaised = true;

            try
            {
                action ();
            }
            finally
            {
                this.eventRaised = false;
            }
        }
        else
        {
            action ();
        }
    }


    private void InnerSubscribe (INotifyPropertyChanged source, INode node, IList<Func<IDisposable>> lastSubscribeActions)
    {
        if (source == null)
        {
            throw new ArgumentNullException ("source");
        }

        if (node == null)
        {
            throw new ArgumentNullException ("node");
        }


        foreach (var link in node.Links)
        {
            switch (link.Type)
            {
                case LinkNodeType.Subscribe:
                {
                    var linkNode = (ILinkSubscribeNode)link;

                    var subscribePropertyRequest = from e in source.GetPropertyChangedEvents ()
                                                   where e.EventArgs.PropertyName == linkNode.Property.Name
                                                   select e;

                    this.items.Add (subscribePropertyRequest.Subscribe (e => this.ProxyEvent(() => linkNode.PropertyChanged(source, e.EventArgs))));
                }
                    break;

                case LinkNodeType.SubscribeSelf:
                {
                    var linkNode = (ILinkNotifyNode)link;

                    this.items.Add(source.GetPropertyChangedEvents().Subscribe(e => this.ProxyEvent(() => linkNode.PropertyChanged(source, e.EventArgs))));
                }
                    break;

                case LinkNodeType.Select:
                {
                    var linkNode = (ILinkNodeToNode)link;

                    var targetSource = (INotifyPropertyChanged)linkNode.Property.GetValue (source, null);

                    if (targetSource != null)
                    {
                        this.InnerSubscribe(targetSource, linkNode.TargetNode, lastSubscribeActions);
                    }

                    lastSubscribeActions.Add (() => (from e in source.GetPropertyChangedEvents ()
                                                     where e.EventArgs.PropertyName == linkNode.Property.Name
                                                     select e).Subscribe (_ => this.RefreshSubscribe ()));
                }
                    break;

                case LinkNodeType.SelectMany:
                {
                    var linkNode = (ILinkNodeToNode)link;

                    var targetCollection = linkNode.Property.GetValue (source, null);

                    if (targetCollection != null)
                    {
                        foreach (var innerSource in DownCastToObservableCollection (targetCollection))
                        {
                            if (innerSource != null)
                            {
                                this.InnerSubscribe(innerSource, linkNode.TargetNode, lastSubscribeActions);
                            }
                        }

                        lastSubscribeActions.Add (() => (targetCollection as INotifyPropertyChanged)
                                                        .GetPropertyChangedEvents ()
                                                        .Subscribe (_ => this.RefreshSubscribe ()));
                    }


                    lastSubscribeActions.Add (() => (from e in source.GetPropertyChangedEvents ()
                                                     where e.EventArgs.PropertyName == linkNode.Property.Name
                                                     select e).Subscribe (_ => this.RefreshSubscribe ()));
                }
                    break;

                default:
                {
                    throw new InvalidOperationException ("unknown link type");
                }
            }
        }
    }



    public void Dispose () => this.UnSubscribe();

    private static IEnumerable<INotifyPropertyChanged> DownCastToObservableCollection(object collection)
    {
#if SILVERLIGHT
            return ((System.Collections.IEnumerable)collection).Cast<INotifyPropertyChanged> (); // IEnumerable<T> в 4 сильверлайте не ковариантный
#else
        return (IEnumerable<INotifyPropertyChanged>) collection;
#endif
    }

}
