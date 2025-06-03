using System;

namespace Anon.RQ_Calc.WPF
{
    public partial class BuffsWindow : IModelContainer<BuffsWindowModel>
    {
        public BuffsWindow()
        {
            this.InitializeComponent();
        }


        public BuffsWindowModel Model
        {
            get { return (BuffsWindowModel)this.DataContext; }
            set { this.DataContext = value; }
        }


        private void DialogControl_OnClosed(object sender, EventArgs<bool> e)
        {
            this.DialogResult = e.Data;
        }
    }
}