using System.Windows;
using System.Windows.Documents;
using RqCalc.Wpf.Models.Window.Dialog;
using RqCalc.Wpf.Windows.Dialog._Base;

namespace RqCalc.Wpf.Windows.Dialog;

public partial class AboutWindow : IModelContainer<AboutWindowModel>
{
    public AboutWindow() => this.InitializeComponent();

    public AboutWindowModel Model
    {
        get => (AboutWindowModel)this.DataContext;
        set => this.DataContext = value;
    }


    private void Button_Link_Click(object sender, RoutedEventArgs e) => System.Diagnostics.Process.Start(((Hyperlink)sender).NavigateUri.ToString());

    private void Button_Click(object sender, RoutedEventArgs e) => this.DialogResult = true;
}