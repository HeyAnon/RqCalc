using System;
using System.Globalization;
using System.Windows.Data;

using Framework.Core;

namespace Anon.RQ_Calc.WPF
{
    public class LegacyModelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value as ILegacyModel).Maybe(model => model.HasLegacy);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}