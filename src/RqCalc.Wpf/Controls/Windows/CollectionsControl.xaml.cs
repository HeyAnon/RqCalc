using System.Windows;
using System.Windows.Controls;

namespace RqCalc.Wpf.Controls.Windows;

public partial class CollectionsControl : UserControl
{
    public CollectionsControl() => this.InitializeComponent();

    private void TabControl_OnLoaded(object sender, RoutedEventArgs e)
    {
        var tabControl = (TabControl)sender;

        if (tabControl.Items.Count > 0)
        {
            tabControl.SelectedItem = tabControl.Items[0];
        }
    }
}