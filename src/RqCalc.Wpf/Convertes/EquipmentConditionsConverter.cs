using System.Globalization;
using System.Windows.Data;
using Framework.Core;
using RqCalc.Domain._Extensions;
using RqCalc.Domain.Equipment;

namespace RqCalc.Wpf.Convertes
{
    public class EquipmentClassConditionsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IEquipment)
            {
                var equipment = value as IEquipment;

                var classes = equipment.GetClassConditions().ToList();

                return !classes.Any() ? "Ыўсющ" : classes.Join(", ", @class => @class.Name);
            }
            else
            {
                return "";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}