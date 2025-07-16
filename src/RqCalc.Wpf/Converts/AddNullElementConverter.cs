using Framework.Core;

namespace RqCalc.Wpf.Converts;

public class AddNullElementConverter : CollectionModifyConverter
{
    protected override object Modify<T>(IEnumerable<T> source) => new[] { default(T) }.Concat(source).ToObservableCollection();
}