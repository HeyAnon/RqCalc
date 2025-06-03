using System.Collections.Generic;
using System.Linq;

using Framework.Core;

namespace Anon.RQ_Calc.WPF
{
    public class AddNullElementConverter : CollectionModifyConverter
    {
        protected override object Modify<T>(IEnumerable<T> source)
        {
            return new[] { default(T) }.Concat(source).ToObservableCollection();
        }
    }
}