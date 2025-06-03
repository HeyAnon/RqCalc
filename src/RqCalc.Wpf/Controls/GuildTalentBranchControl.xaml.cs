using System.Windows;
using System.Windows.Input;

namespace Anon.RQ_Calc.WPF
{
    public partial class GuildTalentBranchControl
    {
        public GuildTalentBranchControl()
        {
            this.InitializeComponent();
        }



        private void Talent_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var control = (FrameworkElement)sender;

            var talentModel = (GuildTalentModel)control.DataContext;

            talentModel.Change(true);
        }

        private void Talent_OnMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var control = (FrameworkElement)sender;

            var talentModel = (GuildTalentModel)control.DataContext;

            talentModel.Change(false);
        }
    }
}