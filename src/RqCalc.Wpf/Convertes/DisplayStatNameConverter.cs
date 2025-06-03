using System;
using System.Globalization;
using System.Windows.Data;

using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.WPF
{
    public class DisplayStatNameConverter : IValueConverter
    {
        public object Convert(object obj, Type targetType, object parameter, CultureInfo culture)
        {
            if (obj is IStat)
            {
                var stat = obj as IStat;

                return stat.GetNameObj().Name;
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

        public static readonly DisplayStatNameConverter Instance = new DisplayStatNameConverter();
    }
}