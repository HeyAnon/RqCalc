using System.ComponentModel;

namespace Framework.Reactive.ObservableRecurse;

public interface INode
{
    IEnumerable<ILinkNode> Links
    {
        get;
    }
}

public interface INode<out TSource> : INode
    where TSource : INotifyPropertyChanged
{

}

public class Node<TSource> : INode<TSource>
    where TSource : INotifyPropertyChanged
{
    public Node (IEnumerable<ILinkNode> links)
    {
        if (links == null)
        {
            throw new ArgumentNullException ("links");
        }

        this.Links = links.ToArray();
    }

    public IEnumerable<ILinkNode> Links
    {
        get;
    }
}
