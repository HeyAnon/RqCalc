using System;
using System.Globalization;
using System.Windows.Data;

using Framework.Core;

namespace Anon.RQ_Calc.WPF
{
    public class TupleWrapConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Tuple.Create(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value as Tuple<object>).Maybe(v => v.Item1, value);
        }
    }
}