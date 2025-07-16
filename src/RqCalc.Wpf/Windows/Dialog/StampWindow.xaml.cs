using RqCalc.Wpf.Models.Window.Dialog;
using RqCalc.Wpf.Windows.Dialog._Base;

namespace RqCalc.Wpf.Windows.Dialog;

public partial class StampWindow : IModelContainer<StampWindowModel>
{
    public StampWindow() => this.InitializeComponent();

    public StampWindowModel Model
    {
        get => (StampWindowModel)this.DataContext;
        set => this.DataContext = value;
    }


    private void DialogControl_OnClosed(object sender, EventArgs<bool> e) => this.DialogResult = e.Data;
}