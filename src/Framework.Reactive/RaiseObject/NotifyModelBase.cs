using System.ComponentModel;
using System.Runtime.Serialization;

using Framework.Core;

namespace Framework.Reactive;

[DataContract(Namespace = "")]
public abstract class NotifyModelBase : BaseRaiseObject, INotifyPropertyChanging
{
    private Dictionary<string, object?> properties;


    protected Dictionary<string, object?> Properties => this.properties ??= new();

    //protected void ClearProperty(string propertyName)
    //{
            
    //}


    protected void RaisePropertyChanging(string propertyName) => this.PropertyChanging.Maybe(v => v(this, new(propertyName)));

    protected internal virtual TProperty? GetGenericValue<TProperty>(string propertyName) => this.Properties.GetMaybeValue(propertyName).Select(v => (TProperty?)v).GetValueOrDefault();

    protected internal virtual bool SetGenericValue<TProperty>(string propertyName, TProperty value)
    {
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
        this.RaisePropertyChanging(propertyName);

        action();

        this.RaisePropertyChanged(propertyName);
    }


    public event PropertyChangingEventHandler? PropertyChanging;
}
