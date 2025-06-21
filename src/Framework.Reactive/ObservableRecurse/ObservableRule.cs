using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Framework.Reactive.ObservableRecurse
{
    public class ObservableRule<TSource>
        where TSource : class, INotifyPropertyChanged
    {
        private ObservableRule ()
        {

        }


        public LinkSelectNode<TSource, TChild> Select<TChild> (Expression<Func<TSource, TChild>> selector, params Func<ObservableRule<TChild>, ILinkNode>[] subRules)
            where TChild : class, INotifyPropertyChanged
        {
            var request = from subRule in subRules
                          select subRule (ObservableRule<TChild>.Value);

            var targetNode = new Node<TChild> (request);

            return new LinkSelectNode<TSource, TChild> (selector, targetNode);
        }



        public LinkSelectManyNode<TSource, TChild> SelectMany<TChild> (Expression<Func<TSource, ObservableCollection<TChild>>> selector, params Func<ObservableRule<TChild>, ILinkNode>[] subRules)
            where TChild : class, INotifyPropertyChanged
        {
            var request = from subRule in subRules
                          select subRule (ObservableRule<TChild>.Value);

            var targetNode = new Node<TChild> (request);

            return new LinkSelectManyNode<TSource, TChild> (selector, targetNode);
        }

        
        public LinkSubscribeNode<TSource, TProperty> Subscribe<TProperty> (Expression<Func<TSource, TProperty>> selectPropertyExpr, PropertyChangedEventHandler notifyPropertyChanged)
        {
            return new LinkSubscribeNode<TSource, TProperty> (selectPropertyExpr, notifyPropertyChanged);
        }

        public LinkSubscribeNode<TSource, TProperty> Subscribe<TProperty>(Expression<Func<TSource, TProperty>> selectPropertyExpr, Action<TSource> notifyPropertyChanged)
        {
            return this.Subscribe(selectPropertyExpr, (sender, e) => notifyPropertyChanged ((TSource)sender));
        }

        public LinkSubscribeNode<TSource, TProperty> Subscribe<TProperty>(Expression<Func<TSource, TProperty>> selectPropertyExpr, Action notifyPropertyChanged)
        {
            return this.Subscribe(selectPropertyExpr, _ => notifyPropertyChanged());
        }

        public LinkSubscribeSelfNode<TSource> SubscribeSelf (PropertyChangedEventHandler notifyPropertyChanged)
        {
            return new LinkSubscribeSelfNode<TSource> (notifyPropertyChanged);
        }

        public LinkSubscribeSelfNode<TSource> SubscribeSelf(Action<TSource> notifyPropertyChanged)
        {
            return new LinkSubscribeSelfNode<TSource>((sender, e) => notifyPropertyChanged((TSource)sender));
        }

        public LinkSubscribeSelfNode<TSource> SubscribeSelf(Action notifyPropertyChanged)
        {
            return new LinkSubscribeSelfNode<TSource>((sender, e) => notifyPropertyChanged());
        }

        public static readonly ObservableRule<TSource> Value = new ObservableRule<TSource> ();
    }


    //public delegate void PropertyChangedEventHandler<T> (object sender, PropertyChangedEventArgs<T> e)
    //    where T : class, INotifyPropertyChanged;


    //public class PropertyChangedEventArgs<T> : EventArgs
    //    where T : class, INotifyPropertyChanged
    //{
    //    public readonly T Source;

    //    public readonly PropertyChangedEventArgs BaseEventArgs;

    //    public PropertyChangedEventArgs (T source, PropertyChangedEventArgs baseEventArgs)
    //    {
    //        if (source == null)
    //        {
    //            throw new ArgumentNullException ("source");
    //        }

    //        if (baseEventArgs == null)
    //        {
    //            throw new ArgumentNullException ("baseEventArgs");
    //        }


    //        this.Source = source;
    //        this.BaseEventArgs = baseEventArgs;
    //    }
    //}
}