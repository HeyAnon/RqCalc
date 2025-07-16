using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;


namespace Framework.Reactive.ObservableRecurse;

public interface ILinkNode
{
    LinkNodeType Type { get; }
}

public interface ILinkNodeProperty : ILinkNode
{   
    PropertyInfo Property { get; }
}

public interface ILinkNode<out TSelectorType> : ILinkNodeProperty
    where TSelectorType : LambdaExpression
{
    TSelectorType Selector { get; }
}

public interface ILinkNodeToNode : ILinkNodeProperty
{
    INode TargetNode { get; }
}

public interface ILinkNode<TSource, TProperty> : ILinkNode<Expression<Func<TSource, TProperty>>>
    where TSource : INotifyPropertyChanged
{
        
}

public interface ILinkNotifyNode : ILinkNode
{
    PropertyChangedEventHandler PropertyChanged { get; }
}

public interface ILinkSubscribeNode : ILinkNodeProperty, ILinkNotifyNode
{
        
}


public enum LinkNodeType
{
    Subscribe,
    SubscribeSelf,
    Select,
    SelectMany
}
