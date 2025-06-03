using System;

namespace Anon.RQ_Calc.WPF
{
    public partial class ConsumablesWindow : IModelContainer<ConsumablesWindowModel>
    {
        public ConsumablesWindow()
        {
            this.InitializeComponent();
        }


        public ConsumablesWindowModel Model
        {
            get { return (ConsumablesWindowModel)this.DataContext; }
            set { this.DataContext = value; }
        }


        private void DialogControl_OnClosed(object sender, EventArgs<bool> e)
        {
            this.DialogResult = e.Data;
        }
    }
}