using System.Collections.Generic;



using Anon.RQ_Calc.Domain;
using Anon.RQ_Calc.Logic;

namespace Anon.RQ_Calc.WPF
{
    public class ActiveImageChangeModelCollection<T, TItem> : ActiveImageChangeModelItem<T, IReadOnlyList<TItem>>
        where T : class, Domain.IImageObject
    {
        public ActiveImageChangeModelCollection(IApplicationContext context, IImage defaultImage = null)
            : base(context, defaultImage)
        {
            this.Value = new TItem[0];
        }
    }
}