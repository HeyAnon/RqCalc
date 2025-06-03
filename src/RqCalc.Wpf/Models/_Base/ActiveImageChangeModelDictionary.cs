using System.Collections.Generic;



using Anon.RQ_Calc.Domain;
using Anon.RQ_Calc.Logic;

namespace Anon.RQ_Calc.WPF
{
    public class ActiveImageChangeModelDictionary<T, TKey, TValue> : ActiveImageChangeModelItem<T, IReadOnlyDictionary<TKey, TValue>>
        where T : class, Domain.IImageObject
    {
        public ActiveImageChangeModelDictionary(IApplicationContext context, IImage defaultImage = null) 
            : base(context, defaultImage)
        {
            this.Value = new Dictionary<TKey, TValue>();
        }
    }
}