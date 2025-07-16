using System.Collections;
using System.ComponentModel;
using System.Reflection;
using Framework.Core;

namespace Framework.Reactive.ObservableRecurse;

internal class SubscribeTotalState : IDisposable
{
    private readonly INotifyPropertyChanged source;
    private readonly PropertyChangedEventHandler handler;

    private readonly List<IDisposable> items = [];


    public SubscribeTotalState (INotifyPropertyChanged source, PropertyChangedEventHandler handler)
    {
        if (source == null)
        {
            throw new ArgumentNullException ("source");
        }

        if (handler == null)
        {
            throw new ArgumentNullException ("handler");
        }


        this.source = source;
        this.handler = handler;

        this.Subscribe ();
    }

    private void Subscribe () => this.InnerSubscribe (this.source, new HashSet<RefWrapper<INotifyPropertyChanged>> ());

    private void UnSubscribe ()
    {
        this.items.ForEach (v => v.Dispose ());
        this.items.Clear ();
    }

    private void RefreshSubscribe ()
    {
        this.UnSubscribe ();
        this.Subscribe ();
    }


    private void InnerSubscribe (INotifyPropertyChanged source, ISet<RefWrapper<INotifyPropertyChanged>> graph)
    {
        if (source == null)
        {
            throw new ArgumentNullException ("source");
        }


        var wrapper = new RefWrapper<INotifyPropertyChanged> (source);

        if (!graph.Add (wrapper))
        {
            return;
        }

        var sourceType = source.GetType ();


        this.items.Add (source.GetPropertyChangedEvents ().Subscribe (e => {
                                                                          this.RefreshSubscribe ();
                                                                          this.handler (source, e.EventArgs);
                                                                      }));

            
            
            


        var subItemsRequest = sourceType.IsObservableCollection()
                                  ? (IEnumerable)source
                                  : source.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => !p.GetIndexParameters().Any()).Select(
                                      property =>
                                          property.GetValue (source, null));

            
        var subItems = subItemsRequest.OfType<INotifyPropertyChanged> ().ToArray ();


        subItems.Foreach (subItem => this.InnerSubscribe (subItem, graph));
    }

    public void Dispose () => this.UnSubscribe ();
}
