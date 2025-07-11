using System.Globalization;
using System.Windows.Data;
using RqCalc.Wpf.Controls;

namespace RqCalc.Wpf.Converts;

/// <summary>
/// �������. ������� �������������� ������ ��� ����� ��� ��������� ���������� ���� � ���� ������(����� ���� -> ��� -> ������)
/// </summary>
public class EquipmentToolTipControlFactoryConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return new EquipmentToolTipControl { Width = 330, DataContext = value };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}