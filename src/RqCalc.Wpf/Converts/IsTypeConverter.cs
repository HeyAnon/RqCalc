using System.Globalization;
using System.Windows.Data;
using Framework.Core;

namespace RqCalc.Wpf.Converts;

public class IsTypeConverter : IValueConverter
{
    public Type Type { get; set; }


    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var request = from type in this.Type.ToMaybe()

            from obj in value.ToMaybe()

            select type.IsInstanceOfType(obj);


        return request.GetValueOrDefault();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}