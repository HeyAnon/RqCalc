using Framework.Core;

namespace RqCalc.Wpf.Convertes
{
    public class AddNullElementConverter : CollectionModifyConverter
    {
        protected override object Modify<T>(IEnumerable<T> source)
        {
            return new[] { default(T) }.Concat(source).ToObservableCollection();
        }
    }
}