using System.Globalization;
using System.Windows.Data;
using Framework.Core;

namespace RqCalc.Wpf.Converts;

public class EnumerableConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => Enumerable.Range(0, (int) value + 1).ToObservableCollection();

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}