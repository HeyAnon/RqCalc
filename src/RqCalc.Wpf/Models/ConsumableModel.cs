

using Anon.RQ_Calc.Domain;
using Anon.RQ_Calc.Logic;

namespace Anon.RQ_Calc.WPF
{
    public class ConsumableModel : ActiveImageChangeModel<IConsumable>
    {
        public ConsumableModel(IApplicationContext context)
            : base(context)
        {
        }
    }
}