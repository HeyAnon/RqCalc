using System.Globalization;
using System.Windows.Data;
using Framework.Core;

namespace RqCalc.Wpf.Converts;

public abstract class CollectionModifyConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var genericType = value?.GetType().GetCollectionElementType();

        if (genericType != null)
        {
            return new Func<IEnumerable<object>, object>(this.Modify)
                .CreateGenericMethod(genericType)
                .Invoke(this, [value]);
        }
        else
        {
            return value;
        }
    }

    protected abstract object Modify<T>(IEnumerable<T> source);



    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotImplementedException();
}