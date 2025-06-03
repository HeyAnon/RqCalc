using System;


using Framework.Reactive;

using Anon.RQ_Calc.Domain;
using Anon.RQ_Calc.Logic;

namespace Anon.RQ_Calc.WPF
{
    public interface IStackObjectModel<out T>
    {
        T SelectedObject { get; }

        int Value { get; }
    }

    public class StackObjectModel<T> : ContextModel, IStackObjectModel<T>
        where T : class, Domain.IImageObject, IStackObject
    {
        public StackObjectModel(IApplicationContext context, T selectObject)
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
}