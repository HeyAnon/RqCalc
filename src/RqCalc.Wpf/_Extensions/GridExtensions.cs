using System.Windows;
using System.Windows.Controls;

namespace RqCalc.Wpf._Extensions;

public static class GridExtensions
{
    public static void AddChild(this Grid grid, UIElement control, int rowIndex, int columnIndex)
    {
        if (grid == null) throw new ArgumentNullException(nameof(grid));
        if (control == null) throw new ArgumentNullException(nameof(control));

        Grid.SetRow(control, rowIndex);
        Grid.SetColumn(control, columnIndex);
            
        grid.Children.Add(control);
    }
}