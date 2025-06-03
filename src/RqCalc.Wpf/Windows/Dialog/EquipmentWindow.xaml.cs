using System;

namespace Anon.RQ_Calc.WPF
{
    public partial class EquipmentWindow : IModelContainer<EquipmentWindowModel>
    {
        public EquipmentWindow()
        {
            this.InitializeComponent();
        }


        public EquipmentWindowModel Model
        {
            get { return (EquipmentWindowModel)this.DataContext; }
            set { this.DataContext = value; }
        }


        private void DialogControl_OnClosed(object sender, EventArgs<bool> e)
        {
            this.DialogResult = e.Data;
        }
    }
}