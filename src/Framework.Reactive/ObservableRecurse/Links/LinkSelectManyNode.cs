using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq.Expressions;


namespace Framework.Reactive.ObservableRecurse;

public class LinkSelectManyNode<TSource, TProperty>(Expression<Func<TSource, ObservableCollection<TProperty>>> selector, INode<TProperty> targetNode)
    : LinkNode<TSource, ObservableCollection<TProperty>>(selector), ILinkNodeToNode
    where TSource : INotifyPropertyChanged
    where TProperty : INotifyPropertyChanged
{
    public INode<TProperty> TargetNode { get; } = targetNode;

    public override LinkNodeType Type => LinkNodeType.SelectMany;

    INode ILinkNodeToNode.TargetNode => this.TargetNode;
}
