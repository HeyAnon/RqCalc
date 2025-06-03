using System;
using System.Collections.ObjectModel;
using System.Windows;
using Anon.RQ_Calc.Domain;
using Framework.Reactive;

namespace Anon.RQ_Calc.WPF
{
    public partial class LoadWindow : Window
    {
        public LoadWindow()
        {
            this.InitializeComponent();

            this.Model = new LoadWindowModel();
        }


        public void SetVersion(Version version)
        {
            this.Model.Version = version;
        }

        public void SetStatus(string status)
        {
            this.Model.Status = status;
        }

        private LoadWindowModel Model
        {
            get => (LoadWindowModel) this.DataContext;
            set => this.DataContext = value;
        }


        private class LoadWindowModel : NotifyModelBase
        {
            public string Status
            {
                get { return this.GetValue(v => v.Status); }
                set { this.SetValue(v => v.Status, value); }
            }

            public Version Version
            {
                get { return this.GetValue(v => v.Version); }
                set { this.SetValue(v => v.Version, value); }
            }
        }
    }
}
