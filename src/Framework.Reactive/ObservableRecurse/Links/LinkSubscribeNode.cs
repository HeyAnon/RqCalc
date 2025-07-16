using System.ComponentModel;
using System.Linq.Expressions;


namespace Framework.Reactive.ObservableRecurse;

public class LinkSubscribeNode<TSource, TProperty>(Expression<Func<TSource, TProperty>> selector, PropertyChangedEventHandler notifyPropertyChanged)
    : LinkNode<TSource, TProperty>(selector), ILinkSubscribeNode
    where TSource : INotifyPropertyChanged
{
    public PropertyChangedEventHandler PropertyChanged
    {
        get;
    } = notifyPropertyChanged;

    public override LinkNodeType Type => LinkNodeType.Subscribe;
}
