using Framework.Core;

namespace RqCalc.Wpf.Converts;

public class ToTupleElementsConverter : CollectionModifyConverter
{
    protected override object Modify<T>(IEnumerable<T> source) => source.Select(v => Tuple.Create<object>(v)).ToObservableCollection();
}