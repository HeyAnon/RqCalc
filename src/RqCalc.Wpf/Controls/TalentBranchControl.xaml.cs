using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using RqCalc.Wpf.Models;

namespace RqCalc.Wpf.Controls;

public partial class TalentBranchControl : UserControl
{
    public TalentBranchControl() => this.InitializeComponent();

    private void Talent_OnMouseButtonDown(object sender, MouseButtonEventArgs e)
    {
        var control = (FrameworkElement)sender;

        var talentModel = (TalentModel)control.DataContext;

        talentModel.SwitchActive();
    }
}