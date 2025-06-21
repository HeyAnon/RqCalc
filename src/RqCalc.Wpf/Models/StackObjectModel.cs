using Framework.Reactive;
using RqCalc.Domain._Base;

namespace RqCalc.Wpf.Models;

public interface IStackObjectModel<out T>
{
    T SelectedObject { get; }

    int Value { get; }
}

public class StackObjectModel<T> : NotifyModelBase, IStackObjectModel<T>
    where T : class, IImageObject, IStackObject
{
    public StackObjectModel(T selectObject)
        : base (context)
    {
        this.SelectedObject = selectObject ?? throw new ArgumentNullException(nameof(selectObject));
    }
        

    public T SelectedObject
    {
        get { return this.GetValue(v => v.SelectedObject); }
        private set { this.SetValue(v => v.SelectedObject, value); }
    }

    public int Value
    {
        get { return this.GetValue(v => v.Value); }
        set { this.SetValue(v => v.Value, value); }
    }
}