using System;

namespace Anon.RQ_Calc.WPF
{
    public partial class StampWindow : IModelContainer<StampWindowModel>
    {
        public StampWindow()
        {
            this.InitializeComponent();
        }


        public StampWindowModel Model
        {
            get { return (StampWindowModel)this.DataContext; }
            set { this.DataContext = value; }
        }


        private void DialogControl_OnClosed(object sender, EventArgs<bool> e)
        {
            this.DialogResult = e.Data;
        }
    }
}