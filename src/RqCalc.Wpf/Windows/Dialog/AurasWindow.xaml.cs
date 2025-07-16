using RqCalc.Wpf.Models.Window.Dialog;
using RqCalc.Wpf.Windows.Dialog._Base;

namespace RqCalc.Wpf.Windows.Dialog;

public partial class AurasWindow : IModelContainer<AurasWindowModel>
{
    public AurasWindow() => this.InitializeComponent();

    public AurasWindowModel Model
    {
        get => (AurasWindowModel)this.DataContext;
        set => this.DataContext = value;
    }


    private void DialogControl_OnClosed(object sender, EventArgs<bool> e) => this.DialogResult = e.Data;
}