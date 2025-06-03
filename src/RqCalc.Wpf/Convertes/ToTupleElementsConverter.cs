using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Core;

namespace Anon.RQ_Calc.WPF
{
    public class ToTupleElementsConverter : CollectionModifyConverter
    {
        protected override object Modify<T>(IEnumerable<T> source)
        {
            return source.Select(v => Tuple.Create<object>(v)).ToObservableCollection();
        }
    }
}