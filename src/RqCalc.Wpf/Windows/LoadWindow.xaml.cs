using System.Windows;
using Framework.Reactive;

namespace RqCalc.Wpf.Windows;

public partial class LoadWindow : Window
{
    public LoadWindow()
    {
        this.InitializeComponent();

        this.Model = new LoadWindowModel();
    }


    public void SetVersion(Version version) => this.Model.Version = version;

    public void SetStatus(string status) => this.Model.Status = status;

    private LoadWindowModel Model
    {
        get => (LoadWindowModel)this.DataContext;
        set => this.DataContext = value;
    }


    private class LoadWindowModel : NotifyModelBase
    {
        public string Status
        {
            get => this.GetValue(v => v.Status);
            set => this.SetValue(v => v.Status, value);
        }

        public Version Version
        {
            get => this.GetValue(v => v.Version);
            set => this.SetValue(v => v.Version, value);
        }
    }
}