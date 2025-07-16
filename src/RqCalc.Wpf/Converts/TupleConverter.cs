using System.Globalization;
using System.Windows.Data;
using Framework.Core;

namespace RqCalc.Wpf.Converts;

public class TupleWrapConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => Tuple.Create(value);

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => (value as Tuple<object>).Maybe(v => v.Item1, value);
}