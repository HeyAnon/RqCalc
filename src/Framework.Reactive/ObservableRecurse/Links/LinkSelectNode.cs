using System.ComponentModel;
using System.Linq.Expressions;


namespace Framework.Reactive.ObservableRecurse;

public class LinkSelectNode<TSource, TProperty>(Expression<Func<TSource, TProperty>> selector, INode<TProperty> targetNode)
    : LinkNode<TSource, TProperty>(selector), ILinkNodeToNode
    where TSource : INotifyPropertyChanged
    where TProperty : INotifyPropertyChanged
{
    public INode<TProperty> TargetNode
    {
        get;
    } = targetNode;

    public override LinkNodeType Type => LinkNodeType.Select;

    INode ILinkNodeToNode.TargetNode => this.TargetNode;
}
