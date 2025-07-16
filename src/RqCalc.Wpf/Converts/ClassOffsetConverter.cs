using System.Globalization;
using System.Windows.Data;
using RqCalc.Domain;
using RqCalc.Domain._Extensions;

namespace RqCalc.Wpf.Converts;

public class ClassOffsetConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is IClass)
        {
            var @class = value as IClass;

            return new string(' ', @class.GetLevelOffset()) + @class.Name;
        }
        else
        {
            return "";
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}