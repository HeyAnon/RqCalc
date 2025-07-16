using System.Globalization;
using System.Windows.Data;

namespace RqCalc.Wpf.Converts;

public class EquipmentRoleNameConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) => (bool)value! ? "Оружие" : "Экипировка";

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotImplementedException();
}