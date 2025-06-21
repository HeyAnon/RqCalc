using System.ComponentModel;
using System.Reactive.Linq;

namespace Framework.Reactive.ObservableRecurse
{
    internal class SubscribeState<TSource> : IDisposable
        where TSource : class, INotifyPropertyChanged
    {
        private readonly TSource _source;
        private readonly Func<ObservableRule<TSource>, ILinkNode>[] _subRules;

        private readonly List<IDisposable> _items = new List<IDisposable> ();

        private readonly bool _lockEvents;
        private bool _eventRaised;
        
        public SubscribeState (TSource source, Func<ObservableRule<TSource>, ILinkNode>[] subRules, bool lockEvents)
        {
            this._source = source;
            this._subRules = subRules;
            this._lockEvents = lockEvents;

            this.Subscribe ();
        }

        private void Subscribe ()
        {
            var rootNode = new Node<TSource> (from subRule in this._subRules
                                              select subRule (ObservableRule<TSource>.Value));


            var lastSubscribeActions = new List<Func<IDisposable>> ();

            this.InnerSubscribe(this._source, rootNode, lastSubscribeActions);

            lastSubscribeActions.ForEach (v => this._items.Add (v ()));
        }

        private void UnSubscribe ()
        {
            this._items.ForEach (v => v.Dispose ());
            this._items.Clear ();
        }

        private void RefreshSubscribe ()
        {
            this.UnSubscribe();
            this.Subscribe();
        }

        private void ProxyEvent (Action action)
        {
            if (action == null) throw new ArgumentNullException("action");

            if (this._lockEvents)
            {
                if (this._eventRaised)
                {
                    return;
                }

                this._eventRaised = true;

                try
                {
                    action ();
                }
                finally
                {
                    this._eventRaised = false;
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

                            this._items.Add (subscribePropertyRequest.Subscribe (e => this.ProxyEvent(() => linkNode.PropertyChanged(source, e.EventArgs))));
                        }
                        break;

                    case LinkNodeType.SubscribeSelf:
                        {
                            var linkNode = (ILinkNotifyNode)link;

                            this._items.Add(source.GetPropertyChangedEvents().Subscribe(e => this.ProxyEvent(() => linkNode.PropertyChanged(source, e.EventArgs))));
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



        public void Dispose ()
        {
            this.UnSubscribe();
        }




        private static IEnumerable<INotifyPropertyChanged> DownCastToObservableCollection(object collection)
        {
#if SILVERLIGHT
            return ((System.Collections.IEnumerable)collection).Cast<INotifyPropertyChanged> (); // IEnumerable<T> в 4 сильверлайте не ковариантный
#else
            return (IEnumerable<INotifyPropertyChanged>) collection;
#endif
        }

    }
}