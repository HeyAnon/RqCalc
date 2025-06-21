using Framework.Core;

namespace RqCalc.Wpf.Converts;

public class AddNullElementConverter : CollectionModifyConverter
{
    protected override object Modify<T>(IEnumerable<T> source)
    {
        return new[] { default(T) }.Concat(source).ToObservableCollection();
    }
}