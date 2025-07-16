using System.Globalization;
using System.Windows.Data;
using RqCalc.Domain;

namespace RqCalc.Wpf.Converts;

public class DisplayStatValueConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values != null && values.Length == 2 && values[0] is IStat && values[1] is decimal)
        {
            var stat = (IStat) values[0];
            var statValue = (decimal)values[1];

            var value = Math.Round(statValue, stat.RoundDigits.GetValueOrDefault(stat.IsPercent ? 2 : 0));

            return $"{value}{(stat.IsPercent ? "%" : "")}";
        }
        else
        {
            return null;
        }
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotImplementedException();

    public static readonly DisplayStatValueConverter Instance = new DisplayStatValueConverter();
}

public class DisplayStatDescriptionConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values != null && values.Length == 3 && values[0] is IStat && values[1] is decimal)
        {
            var stat = (IStat)values[0];


            if (!string.IsNullOrEmpty(stat.DescriptionTemplate))
            {
                var statValue = (decimal)values[1];
                var baseDescriptionValue = (decimal?)values[2];

                var descriptionValue = baseDescriptionValue ?? statValue;

                var value = Math.Round(descriptionValue, 2);

                return string.Format(stat.DescriptionTemplate, value);
            }
        }

        return null;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotImplementedException();

    public static readonly DisplayStatDescriptionConverter Instance = new DisplayStatDescriptionConverter();
}