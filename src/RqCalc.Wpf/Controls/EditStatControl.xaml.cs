using System.Windows;
using System.Windows.Controls;
using RqCalc.Wpf.Models;

namespace RqCalc.Wpf.Controls;

public partial class EditStatControl : UserControl
{
    public EditStatControl() => this.InitializeComponent();

    private CharacterStatEditModel Model => (CharacterStatEditModel)this.DataContext;


    private void Button_Up_Click(object sender, RoutedEventArgs e) => this.Model.SetMaxStat();

    private void Button_Plus_MouseClick(object sender, EventArgs e) => this.Model.TryIncreaseStat();

    private void Button_Minus_MouseClick(object sender, EventArgs e) => this.Model.TryDecreaseStat();
}