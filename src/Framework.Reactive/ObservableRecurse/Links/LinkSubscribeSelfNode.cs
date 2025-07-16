using System.ComponentModel;

namespace Framework.Reactive.ObservableRecurse;

public class LinkSubscribeSelfNode<TSource> : ILinkNotifyNode
    where TSource : INotifyPropertyChanged
{
    public LinkSubscribeSelfNode (PropertyChangedEventHandler propertyChanged)
    {
        if (propertyChanged == null)
        {
            throw new ArgumentNullException ("propertyChanged");
        }

        this.PropertyChanged = propertyChanged;
    }


    public PropertyChangedEventHandler PropertyChanged
    {
        get;
    }

    public LinkNodeType Type => LinkNodeType.SubscribeSelf;
}
