using System;

namespace Anon.RQ_Calc.WPF
{
    public partial class GuildTalentsWindow : IModelContainer<GuildTalentsWindowModel>
    {
        public readonly ICodeRouter Router;


        public GuildTalentsWindow(ICodeRouter router)
            : this()
        {
            this.Router = router ?? throw new ArgumentNullException(nameof(router));
        }


        public GuildTalentsWindow()
        {
            this.InitializeComponent();
        }


        public GuildTalentsWindowModel Model
        {
            get { return (GuildTalentsWindowModel)this.DataContext; }
            set { this.DataContext = value; }
        }


        private void DialogControl_OnClosed(object sender, EventArgs<bool> e)
        {
            this.DialogResult = e.Data;
        }
    }
}