using System.Globalization;
using System.Windows.Data;

namespace RqCalc.Wpf.Converts;

public class DebugConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => value;

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => value;
}