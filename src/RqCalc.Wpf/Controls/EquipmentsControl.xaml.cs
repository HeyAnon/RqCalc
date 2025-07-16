using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using RqCalc.Wpf.Models;
using RqCalc.Wpf.Windows.Dialog;
using RqCalc.Wpf.Windows.Dialog._Base;

namespace RqCalc.Wpf.Controls;

public partial class EquipmentsControl : UserControl
{
    public EquipmentsControl() => this.InitializeComponent();

    public CharacterChangeModel Model => (CharacterChangeModel)this.DataContext;

    private void Equipment_Click_MouseDown(object sender, MouseButtonEventArgs e)
    {
        var control = (EquipmentControl)sender;

        var baseEquipmentModel = (EquipmentChangeModel)control.DataContext;

        var editModel = this.Model.GetEquipmentEditModel(baseEquipmentModel);

        var windowModel = this.Model.CreateEquipmentWindowModel(baseEquipmentModel);

        new EquipmentWindow { Model = windowModel, Owner = Window.GetWindow(this) }.SuccessDialog(() =>
            editModel.Data = windowModel.Equipment == null ? null : windowModel);
    }
}