using System.ComponentModel;
using System.Runtime.Serialization;

using Framework.Core;

namespace Framework.Reactive
{
    [DataContract(Namespace = "")]
    public abstract class NotifyModelBase : BaseRaiseObject, INotifyPropertyChanging
    {
        private Dictionary<string, object> _properties;
        

        protected NotifyModelBase()
        {

        }


        protected Dictionary<string, object> Properties => this._properties ?? (this._properties = new Dictionary<string, object>());

        //protected void ClearProperty(string propertyName)
        //{
            
        //}


        protected void RaisePropertyChanging(string propertyName)
        {
            this.PropertyChanging.Maybe(v => v(this, new PropertyChangingEventArgs(propertyName)));
        }


        protected virtual internal TProperty GetGenericValue<TProperty>(string propertyName)
        {
            return this.Properties.GetMaybeValue(propertyName).Select(v => (TProperty)v).GetValueOrDefault();
        }

        protected virtual internal bool SetGenericValue<TProperty>(string propertyName, TProperty value)
        {
            if (propertyName == null) throw new ArgumentNullException("propertyName");
            
            var prevValue = this.GetGenericValue<TProperty>(propertyName);

            if (EqualityComparer<TProperty>.Default.Equals(prevValue, value))
            {
                return false;
            }

            this.RaiseAction(propertyName, () => this.Properties[propertyName] = value);

            return true;
        }

        protected void RaiseAction(string propertyName, Action action)
        {
            if (propertyName == null) throw new ArgumentNullException("propertyName");
            if (action == null) throw new ArgumentNullException("action");

            this.RaisePropertyChanging(propertyName);

            action();

            this.RaisePropertyChanged(propertyName);
        }


        public event PropertyChangingEventHandler PropertyChanging;
    }
}
