using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

using Framework.Core;

namespace Anon.RQ_Calc.WPF
{
    public class EnumerableConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Enumerable.Range(0, (int) value + 1).ToObservableCollection();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}