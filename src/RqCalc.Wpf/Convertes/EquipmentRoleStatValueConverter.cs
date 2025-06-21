using System.Globalization;
using System.Windows.Data;
using Framework.Core;
using RqCalc.Domain;

namespace RqCalc.Wpf.Convertes
{
    public class EquipmentRoleStatValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var request = from equipmentInfo in (value as EquipmentBaseInfo).ToMaybe()

                select equipmentInfo.Match(info => info.Defense, info => info.Attack, info => info.Attack);

            return request.GetValueOrDefault();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}