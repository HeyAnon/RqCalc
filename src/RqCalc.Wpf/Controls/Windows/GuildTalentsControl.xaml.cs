using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using RqCalc.Wpf.Models.Window.Dialog;
using RqCalc.Wpf.Windows.Dialog;

namespace RqCalc.Wpf.Controls.Windows;

public partial class GuildTalentsControl : UserControl
{
    public GuildTalentsControl() => this.InitializeComponent();

    public GuildTalentsWindowModel Model => (GuildTalentsWindowModel)this.DataContext;


    private void TextBox_Code_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        var textBox = (TextBox)sender;

        if (e.Key == Key.Enter)
        {
            var be = textBox.GetBindingExpression(TextBox.TextProperty);

            be?.UpdateSource();
        }
    }

    private void Button_Link_Click(object sender, RoutedEventArgs e)
    {
        var window = Window.GetWindow(this) as GuildTalentsWindow;

        window?.CodeRouter.RouteGuildTalent(this.Model.Code);
    }
}