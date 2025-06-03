using System;

namespace Anon.RQ_Calc.WPF
{
    public partial class ElixirWindow : IModelContainer<ElixirWindowModel>
    {
        public ElixirWindow()
        {
            this.InitializeComponent();
        }


        public ElixirWindowModel Model
        {
            get { return (ElixirWindowModel)this.DataContext; }
            set { this.DataContext = value; }
        }


        private void DialogControl_OnClosed(object sender, EventArgs<bool> e)
        {
            this.DialogResult = e.Data;
        }
    }
}