using System;

namespace Anon.RQ_Calc.WPF
{
    public partial class CardWindow : IModelContainer<CardWindowModel>
    {
        public CardWindow()
        {
            this.InitializeComponent();
        }


        public CardWindowModel Model
        {
            get { return (CardWindowModel)this.DataContext; }
            set { this.DataContext = value; }
        }


        private void DialogControl_OnClosed(object sender, EventArgs<bool> e)
        {
            this.DialogResult = e.Data;
        }
    }
}