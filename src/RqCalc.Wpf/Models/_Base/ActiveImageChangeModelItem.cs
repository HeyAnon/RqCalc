
using Framework.Reactive;

using Anon.RQ_Calc.Domain;
using Anon.RQ_Calc.Logic;

namespace Anon.RQ_Calc.WPF
{
    public class ActiveImageChangeModelItem<T, TValue> : ActiveImageChangeModel<T>
        where T : class, Domain.IImageObject
    {
        public ActiveImageChangeModelItem(IApplicationContext context, IImage defaultImage = null)
            : base(context, defaultImage)
        {
        }


        public TValue Value
        {
            get { return this.GetValue(v => v.Value); }
            set { this.SetValue(v => v.Value, value); }
        }
    }
}