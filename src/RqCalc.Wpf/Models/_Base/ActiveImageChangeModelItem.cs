using Framework.Reactive;
using RqCalc.Domain._Base;

namespace RqCalc.Wpf.Models._Base;

public class ActiveImageChangeModelItem<T, TValue> : ActiveImageChangeModel<T>
    where T : class, IImageObject
{
    public ActiveImageChangeModelItem(IImage? defaultImage = null)
        : base(context, defaultImage)
    {
    }


    public TValue Value
    {
        get { return this.GetValue(v => v.Value); }
        set { this.SetValue(v => v.Value, value); }
    }
}