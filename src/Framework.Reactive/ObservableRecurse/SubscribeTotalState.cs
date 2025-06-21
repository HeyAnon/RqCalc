using System.Collections;
using System.ComponentModel;
using System.Reflection;
using Framework.Core;

namespace Framework.Reactive.ObservableRecurse
{
    internal class SubscribeTotalState : IDisposable
    {
        private readonly INotifyPropertyChanged Source;
        private readonly PropertyChangedEventHandler Handler;

        private readonly List<IDisposable> Items = new List<IDisposable> ();


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


            this.Source = source;
            this.Handler = handler;

            Subscribe ();
        }

        private void Subscribe ()
        {
            InnerSubscribe (this.Source, new HashSet<RefWrapper<INotifyPropertyChanged>> ());
        }

        private void UnSubscribe ()
        {
            this.Items.ForEach (v => v.Dispose ());
            this.Items.Clear ();
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


            this.Items.Add (source.GetPropertyChangedEvents ().Subscribe (e => { RefreshSubscribe ();
                                                                                 this.Handler (source, e.EventArgs);
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