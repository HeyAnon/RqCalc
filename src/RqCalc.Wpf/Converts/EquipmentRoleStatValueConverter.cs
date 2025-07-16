using System.Globalization;
using System.Windows.Data;
using Framework.Core;
using RqCalc.Domain;

namespace RqCalc.Wpf.Converts;

public class EquipmentRoleStatValueConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var request =

            from equipmentInfo in (value as EquipmentBaseInfo).ToMaybe()

            select equipmentInfo switch
            {
                EquipmentInfo info => info.Defense,
                WeaponInfo info => info.Attack,
                DefenseWeaponInfo info => info.Attack,
                _ => throw new NotImplementedException()
            };

        return request.GetValueOrDefault();
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        throw new NotImplementedException();
}