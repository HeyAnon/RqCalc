using System;
using System.Windows;
using System.Windows.Documents;

namespace Anon.RQ_Calc.WPF
{
    public partial class AboutWindow : IModelContainer<AboutWindowModel>
    {
        public AboutWindow()
        {
            this.InitializeComponent();
        }


        public AboutWindowModel Model
        {
            get { return (AboutWindowModel)this.DataContext; }
            set { this.DataContext = value; }
        }


        private void Button_Link_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(((Hyperlink)sender).NavigateUri.ToString());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}