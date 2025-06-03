using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

using Framework.Core;

using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.WPF
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