

using Anon.RQ_Calc.Domain;
using Anon.RQ_Calc.Logic;

namespace Anon.RQ_Calc.WPF
{
    public class BuffModel : StackObjectModel<IBuff>
    {
        public BuffModel(IApplicationContext context, IBuff selectObject)
            : base(context, selectObject)
        {
        }
    }

    //public class BuffModel : StackObjectModel<IBuff>
    //{
    //    public BuffModel(IApplicationContext context, IBuff selectObject)
    //        : base(context, selectObject)
    //    {
    //    }


    //    //public bool IsEnabled
    //    //{
    //    //    get { return this.GetValue(v => v.IsEnabled); }
    //    //    set { this.SetValue(v => v.IsEnabled, value); }
    //    //}
    //}
}