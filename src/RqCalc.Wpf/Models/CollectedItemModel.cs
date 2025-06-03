using Anon.RQ_Calc.Domain;
using Anon.RQ_Calc.Logic;



namespace Anon.RQ_Calc.WPF
{
    public class CollectedItemModel : ActiveImageChangeModel<ICollectedItem>
    {
        public CollectedItemModel(IApplicationContext context)
            : base(context)
        {
        }
    }
}