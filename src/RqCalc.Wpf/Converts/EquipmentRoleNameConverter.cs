using System.Globalization;
using System.Windows.Data;

namespace RqCalc.Wpf.Converts;

public class EquipmentRoleNameConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (bool)value ? "������" : "����������";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}