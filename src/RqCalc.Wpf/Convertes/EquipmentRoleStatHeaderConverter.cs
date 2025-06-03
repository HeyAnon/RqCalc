using System;
using System.Globalization;
using System.Windows.Data;

namespace Anon.RQ_Calc.WPF
{
    public class EquipmentRoleStatHeaderConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? "Атака" : "Защита";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}