using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace Framework.Reactive.ObservableRecurse;

public abstract class LinkNode<TSource, TProperty>(Expression<Func<TSource, TProperty>> selector) : ILinkNode<TSource, TProperty>
    where TSource : INotifyPropertyChanged
{
    public Expression<Func<TSource, TProperty>> Selector{ get; } = selector;

    public abstract LinkNodeType Type { get; }

    public PropertyInfo Property
    {
        get;
    } = selector.GetProperty ();
}
