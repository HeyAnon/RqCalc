using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Framework.Reactive.ObservableRecurse;

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

        return new(selector, targetNode);
    }



    public LinkSelectManyNode<TSource, TChild> SelectMany<TChild> (Expression<Func<TSource, ObservableCollection<TChild>>> selector, params Func<ObservableRule<TChild>, ILinkNode>[] subRules)
        where TChild : class, INotifyPropertyChanged
    {
        var request = from subRule in subRules
                      select subRule (ObservableRule<TChild>.Value);

        var targetNode = new Node<TChild> (request);

        return new(selector, targetNode);
    }

        
    public LinkSubscribeNode<TSource, TProperty> Subscribe<TProperty> (Expression<Func<TSource, TProperty>> selectPropertyExpr, PropertyChangedEventHandler notifyPropertyChanged) => new(selectPropertyExpr, notifyPropertyChanged);

    public LinkSubscribeNode<TSource, TProperty> Subscribe<TProperty>(Expression<Func<TSource, TProperty>> selectPropertyExpr, Action<TSource> notifyPropertyChanged) => this.Subscribe(selectPropertyExpr, (sender, e) => notifyPropertyChanged ((TSource)sender));

    public LinkSubscribeNode<TSource, TProperty> Subscribe<TProperty>(Expression<Func<TSource, TProperty>> selectPropertyExpr, Action notifyPropertyChanged) => this.Subscribe(selectPropertyExpr, _ => notifyPropertyChanged());

    public LinkSubscribeSelfNode<TSource> SubscribeSelf (PropertyChangedEventHandler notifyPropertyChanged) => new(notifyPropertyChanged);

    public LinkSubscribeSelfNode<TSource> SubscribeSelf(Action<TSource> notifyPropertyChanged) => new((sender, e) => notifyPropertyChanged((TSource)sender));

    public LinkSubscribeSelfNode<TSource> SubscribeSelf(Action notifyPropertyChanged) => new((sender, e) => notifyPropertyChanged());

    public static readonly ObservableRule<TSource> Value = new();
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
