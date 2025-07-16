using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using Framework.Core;
using RqCalc.Domain.Stamp;

namespace RqCalc.Wpf.Converts;

public class StampColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var brushRequest = from stampColor in (value as IStampColor).ToMaybe()

            let brush = new BrushConverter().ConvertFrom(stampColor.Argb)

            select (SolidColorBrush)brush;


        return brushRequest.GetValueOrDefault(() => Brushes.White);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}