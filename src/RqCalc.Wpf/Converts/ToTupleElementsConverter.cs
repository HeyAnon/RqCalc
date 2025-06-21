using Framework.Core;

namespace RqCalc.Wpf.Converts;

public class ToTupleElementsConverter : CollectionModifyConverter
{
    protected override object Modify<T>(IEnumerable<T> source)
    {
        return source.Select(v => Tuple.Create<object>(v)).ToObservableCollection();
    }
}