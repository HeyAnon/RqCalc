using System.Windows;
using System.Windows.Controls;
using RqCalc.Wpf.Models;

namespace RqCalc.Wpf.Controls
{
    public partial class EditStatControl : UserControl
    {
        public EditStatControl()
        {
            this.InitializeComponent();
        }


        private CharacterStatEditModel Model => (CharacterStatEditModel)this.DataContext;


        private void Button_Up_Click(object sender, RoutedEventArgs e)
        {
            var limit = this.Model.Context.Settings.MaxStatCount - this.Model.EditValue;
            
            if (limit > 0)
            {
                this.Model.EditValue = this.Model.EditValue + Math.Min(this.Model.RootModel.FreeStats, limit);
            }
        }

        private void Button_Plus_MouseClick(object sender, EventArgs e)
        {
            var limit = this.Model.Context.Settings.MaxStatCount - this.Model.EditValue;

            if (limit > 0 && this.Model.RootModel.FreeStats > 0)
            {
                this.Model.EditValue++;
            }
        }

        private void Button_Minus_MouseClick(object sender, EventArgs e)
        {
            if (this.Model.EditValue > 1)
            {
                this.Model.EditValue--;
            }
        }
    }
}