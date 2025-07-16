using RqCalc.Wpf.Models.Window.Dialog;
using RqCalc.Wpf.Windows.Dialog._Base;
using RqCalc.Wpf.Windows.Route;

namespace RqCalc.Wpf.Windows.Dialog;

public partial class TalentsWindow : IModelContainer<TalentsWindowModel>
{
    public readonly ICodeRouter Router;


    public TalentsWindow(ICodeRouter router)
        : this() =>
        this.Router = router ?? throw new ArgumentNullException(nameof(router));

    public TalentsWindow() => this.InitializeComponent();

    public TalentsWindowModel Model
    {
        get => (TalentsWindowModel)this.DataContext;
        set => this.DataContext = value;
    }


    private void DialogControl_OnClosed(object sender, EventArgs<bool> e) => this.DialogResult = e.Data;
}