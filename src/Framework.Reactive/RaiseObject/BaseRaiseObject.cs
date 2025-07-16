using System.ComponentModel;
using System.Runtime.Serialization;

using Framework.Core;

namespace Framework.Reactive;

[DataContract(Namespace = "")]
public class BaseRaiseObject : IBaseRaiseObject
{
    public event PropertyChangedEventHandler? PropertyChanged;


    protected void RaisePropertyChanged(string propertyName) => (this as IBaseRaiseObject).PropertyChanged.Maybe(v => v(this, new(propertyName)));

    PropertyChangedEventHandler? IBaseRaiseObject.PropertyChanged => this.PropertyChanged;
}
