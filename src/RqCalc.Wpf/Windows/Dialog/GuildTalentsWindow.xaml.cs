using RqCalc.Wpf.Models.Window.Dialog;
using RqCalc.Wpf.Windows.Dialog._Base;
using RqCalc.Wpf.Windows.Route;

namespace RqCalc.Wpf.Windows.Dialog;

public partial class GuildTalentsWindow : IModelContainer<GuildTalentsWindowModel>
{
    public readonly ICodeRouter CodeRouter;

    public GuildTalentsWindow(ICodeRouter codeRouter)
        : this() =>
        this.CodeRouter = codeRouter;

    public GuildTalentsWindow() => this.InitializeComponent();

    public GuildTalentsWindowModel Model
    {
        get => (GuildTalentsWindowModel)this.DataContext;
        set => this.DataContext = value;
    }


    private void DialogControl_OnClosed(object sender, EventArgs<bool> e) => this.DialogResult = e.Data;
}