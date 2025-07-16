using Framework.Reactive;

using RqCalc.Domain._Base;

namespace RqCalc.Wpf.Models._Base;

public class ActiveImageChangeModelItem<T, TValue>(IImage? defaultImage = null) : ActiveImageChangeModel<T>(defaultImage)
    where T : class, IImageObject
{
    public TValue Value
    {
        get => this.GetValue(v => v.Value);
        set => this.SetValue(v => v.Value, value);
    }
}