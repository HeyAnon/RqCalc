using RqCalc.Wpf.Models.Window.Dialog;
using RqCalc.Wpf.Windows.Dialog._Base;

namespace RqCalc.Wpf.Windows.Dialog;

public partial class EquipmentWindow : IModelContainer<EquipmentWindowModel>
{
    public EquipmentWindow() => this.InitializeComponent();

    public EquipmentWindowModel Model
    {
        get => (EquipmentWindowModel)this.DataContext;
        set => this.DataContext = value;
    }


    private void DialogControl_OnClosed(object sender, EventArgs<bool> e) => this.DialogResult = e.Data;
}