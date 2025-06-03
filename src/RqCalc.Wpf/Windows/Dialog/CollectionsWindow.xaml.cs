using System;

namespace Anon.RQ_Calc.WPF
{
    public partial class CollectionsWindow : IModelContainer<CollectionsWindowModel>
    {
        public CollectionsWindow()
        {
            this.InitializeComponent();
        }


        public CollectionsWindowModel Model
        {
            get { return (CollectionsWindowModel)this.DataContext; }
            set { this.DataContext = value; }
        }


        private void DialogControl_OnClosed(object sender, EventArgs<bool> e)
        {
            this.DialogResult = e.Data;
        }
    }
}