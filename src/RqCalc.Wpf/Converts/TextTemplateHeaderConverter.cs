using System.Globalization;
using System.Windows.Data;

namespace RqCalc.Wpf.Converts;

public class TextTemplateHeaderConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string)
        {
            var header = value as string;

            if (!string.IsNullOrWhiteSpace(header))
            {
                return $"{header}: ";
            }
        }

        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}