using System;
using System.Globalization;
using System.Windows.Data;

using Framework.Core;

using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.WPF
{
    public class EquipmentGenderConditionsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IEquipment)
            {
                var equipment = value as IEquipment;

                return equipment.Gender.Maybe(v => v.Name, "Ыўсющ");
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