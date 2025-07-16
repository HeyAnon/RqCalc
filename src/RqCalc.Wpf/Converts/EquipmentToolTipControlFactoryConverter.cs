using System.Globalization;
using System.Windows.Data;

using RqCalc.Wpf.Controls;

namespace RqCalc.Wpf.Converts;

/// <summary>
/// Костыль. Неверно рассчитывается высота под карты при изменении количества карт в типе шмотки (левая рука -> щит -> оружие).
/// </summary>
public class EquipmentToolTipControlFactoryConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) => new EquipmentToolTipControl { Width = 330, DataContext = value };

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotImplementedException();
}