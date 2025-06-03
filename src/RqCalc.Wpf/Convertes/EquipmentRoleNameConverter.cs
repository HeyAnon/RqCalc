using System;
using System.Globalization;
using System.Windows.Data;

namespace Anon.RQ_Calc.WPF
{
    public class EquipmentRoleNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? "Оружие" : "Экипировка";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}