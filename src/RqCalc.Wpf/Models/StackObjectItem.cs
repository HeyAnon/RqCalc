using Framework.Reactive;
using RqCalc.Domain._Base;
using RqCalc.Wpf.Models._Base;

namespace RqCalc.Wpf.Models
{
    public class StackObjectItem<T> : ContextModel
        where T : class, IImageObject, IStackObject, IBonusBase
    {
        public StackObjectItem(IServiceProvider context, T selectObject, int value)
            : base(context)
        {
            if (selectObject == null) throw new ArgumentNullException(nameof(selectObject));

            this.SelectedObject = selectObject;
            this.Value = value;
        }


        public T SelectedObject
        {
            get { return this.GetValue(v => v.SelectedObject); }
            private set { this.SetValue(v => v.SelectedObject, value); }
        }

        public int Value
        {
            get { return this.GetValue(v => v.Value); }
            private set { this.SetValue(v => v.Value, value); }
        }
    }
}