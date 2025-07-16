using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using RqCalc.Wpf.Models;
using RqCalc.Wpf.Models.Window.Dialog;
using RqCalc.Wpf.Windows.Dialog;
using RqCalc.Wpf.Windows.Dialog._Base;

namespace RqCalc.Wpf.Controls.Windows;

public partial class EquipmentSelectorControl : UserControl
{
    public EquipmentSelectorControl() => this.InitializeComponent();

    public EquipmentWindowModel Model => (EquipmentWindowModel)this.DataContext;


    private void Card_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        var control = (EquipmentCardControl)sender;

        var model = (EquipmentCardModel)control.DataContext;

        if (this.Model.Equipment != null && (this.Model.Equipment.PrimaryCard == null || model.Index != 0))
        {
            var windowModel = this.Model.CreateCardWindowModel(model.Index, model.Card);

            new CardWindow { Model = windowModel, Owner = Window.GetWindow(this) }.SuccessDialog(() => model.Card = windowModel.Card);
        }
    }

    private void Stamp_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (this.Model.Equipment != null)
        {
            var windowModel = this.Model.CreateStampWindowModel();

            new StampWindow {Model = windowModel, Owner = Window.GetWindow(this)}.SuccessDialog(() => this.Model.StampVariant = windowModel.StampVariant);
        }
    }
}