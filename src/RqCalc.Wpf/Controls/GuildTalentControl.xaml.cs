using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace Anon.RQ_Calc.WPF
{
    public partial class GuildTalentControl : UserControl
    {
        public GuildTalentControl()
        {
            this.InitializeComponent();
        }

        private async void MainGrid_OnMouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            //if ((sender as Grid)?.ToolTip is ToolTip toolTip)
            //{
            //    await Task.Run(() => Thread.Sleep(100));

            //    toolTip.IsOpen = true;
            //}
        }
    }
}