using System.Globalization;
using System.Windows.Data;
using RqCalc.Domain;
using RqCalc.Wpf._Extensions;

namespace RqCalc.Wpf.Converts;

public class DisplayStatNameConverter : IValueConverter
{
    public object Convert(object obj, Type targetType, object parameter, CultureInfo culture)
    {
        if (obj is IStat)
        {
            var stat = obj as IStat;

            return stat.GetNameObj().Name;
        }
        else
        {
            return "";
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();

    public static readonly DisplayStatNameConverter Instance = new DisplayStatNameConverter();
}