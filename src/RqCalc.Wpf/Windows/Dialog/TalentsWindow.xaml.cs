using System;

namespace Anon.RQ_Calc.WPF
{
    public partial class TalentsWindow : IModelContainer<TalentsWindowModel>
    {
        public readonly ICodeRouter Router;


        public TalentsWindow(ICodeRouter router)
            : this()
        {
            this.Router = router ?? throw new ArgumentNullException(nameof(router));
        }


        public TalentsWindow()
        {
            this.InitializeComponent();
        }


        public TalentsWindowModel Model
        {
            get { return (TalentsWindowModel)this.DataContext; }
            set { this.DataContext = value; }
        }


        private void DialogControl_OnClosed(object sender, EventArgs<bool> e)
        {
            this.DialogResult = e.Data;
        }
    }
}