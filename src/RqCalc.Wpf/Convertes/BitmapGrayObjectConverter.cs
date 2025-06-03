using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

using Framework.Core;

namespace Anon.RQ_Calc.WPF
{
    public class BitmapGrayObjectConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value as BitmapFrame).Maybe(v => v.ToGray());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}