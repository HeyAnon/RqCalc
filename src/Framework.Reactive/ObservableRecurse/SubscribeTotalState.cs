using System.Collections;
using System.ComponentModel;
using System.Reflection;
using Framework.Core;

namespace Framework.Reactive.ObservableRecurse
{
    internal class SubscribeTotalState : IDisposable
    {
        private readonly INotifyPropertyChanged source;
        private readonly PropertyChangedEventHandler handler;

        private readonly List<IDisposable> items = new();


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

            Subscribe ();
        }

        private void Subscribe ()
        {
            InnerSubscribe (this.source, new HashSet<RefWrapper<INotifyPropertyChanged>> ());
        }

        private void UnSubscribe ()
        {
            this.items.ForEach (v => v.Dispose ());
            this.items.Clear ();
        }

        private void RefreshSubscribe ()
        {
            UnSubscribe ();
            Subscribe ();
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


            this.items.Add (source.GetPropertyChangedEvents ().Subscribe (e => { RefreshSubscribe ();
                                                                                 this.handler (source, e.EventArgs);
                                                                               }));

            
            
            


            var subItemsRequest = sourceType.IsObservableCollection()
                          ? (IEnumerable)source
                          : source.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => !p.GetIndexParameters().Any()).Select(
                            property =>
                                property.GetValue (source, null));

            
            var subItems = subItemsRequest.OfType<INotifyPropertyChanged> ().ToArray ();


            subItems.Foreach (subItem => InnerSubscribe (subItem, graph));
        }

        public void Dispose ()
        {
            UnSubscribe ();
        }
    }
}