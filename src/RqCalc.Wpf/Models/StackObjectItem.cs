using Framework.Reactive;
using RqCalc.Domain._Base;

namespace RqCalc.Wpf.Models;

public class StackObjectItem<T> : NotifyModelBase
    where T : class, IImageObject, IStackObject, IBonusBase
{
    public StackObjectItem(T selectObject, int value)

    {
        if (selectObject == null) throw new ArgumentNullException(nameof(selectObject));

        this.SelectedObject = selectObject;
        this.Value = value;
    }


    public T SelectedObject
    {
        get => this.GetValue(v => v.SelectedObject);
        private set => this.SetValue(v => v.SelectedObject, value);
    }

    public int Value
    {
        get => this.GetValue(v => v.Value);
        private set => this.SetValue(v => v.Value, value);
    }
}