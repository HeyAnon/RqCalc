using System;

namespace Anon.RQ_Calc.WPF
{
    public partial class AurasWindow : IModelContainer<AurasWindowModel>
    {
        public AurasWindow()
        {
            this.InitializeComponent();
        }


        public AurasWindowModel Model
        {
            get { return (AurasWindowModel)this.DataContext; }
            set { this.DataContext = value; }
        }


        private void DialogControl_OnClosed(object sender, EventArgs<bool> e)
        {
            this.DialogResult = e.Data;
        }
    }
}