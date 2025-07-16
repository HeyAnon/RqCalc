using System.Globalization;
using System.Windows.Data;

using Framework.Core;

using RqCalc.Domain.Equipment;

namespace RqCalc.Wpf.Converts;

public class EquipmentGenderConditionsConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is IEquipment equipment)
        {
            return equipment.Gender.Maybe(v => v.Name, "Любой");
        }
        else
        {
            return "";
        }
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotImplementedException();
}