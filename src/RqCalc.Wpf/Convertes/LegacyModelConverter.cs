using System.Globalization;
using System.Windows.Data;
using Framework.Core;
using RqCalc.Wpf.Models.Window.Dialog._Base;

namespace RqCalc.Wpf.Convertes
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