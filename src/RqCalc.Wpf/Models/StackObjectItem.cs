using System;


using Framework.Reactive;

using Anon.RQ_Calc.Domain;
using Anon.RQ_Calc.Logic;

namespace Anon.RQ_Calc.WPF
{
    public class StackObjectItem<T> : ContextModel
        where T : class, Domain.IImageObject, IStackObject, IBonusBase
    {
        public StackObjectItem(IApplicationContext context, T selectObject, int value)
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