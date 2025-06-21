using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;
using RqCalc.Wpf.Exception;
using RqCalc.Wpf.Models;
using RqCalc.Wpf.Models.Window;
using RqCalc.Wpf.Models.Window.Dialog;
using RqCalc.Wpf.Windows.Dialog;
using RqCalc.Wpf.Windows.Dialog._Base;
using RqCalc.Wpf.Windows.Route;

namespace RqCalc.Wpf.Windows;

public partial class MainWindow
{
    private readonly ICodeRouter router;


    protected MainWindow()
    {
        this.InitializeComponent();
    }

    public MainWindow(Version version, IApplicationSettings settings, string startupCode)
        : this()
    {
        if (context == null) throw new ArgumentNullException(nameof(context));
        if (settings == null) throw new ArgumentNullException(nameof(settings));

        this.router = new WebRouter(settings);
        this.DataContext = new MainWindowModel(context, version);

        if (startupCode != null)
        {
            this.Model.Character = new CharacterChangeModel(this.Context, this.Context.Serializer.Deserialize(startupCode));
        }
    }


    private MainWindowModel Model => (MainWindowModel)this.DataContext;

    private IServiceProvider Context => this.Model.Context;


    private void Button_Link_Click(object sender, RoutedEventArgs e)
    {
        this.router.RouteMain(this.Model.Character.Code);
    }

    private void Button_Save_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new SaveFileDialog
        {
            Filter = "RQCalc Files|*.rqcalc|All Files (*.*)|*.*"
        };

        if (dialog.ShowDialog() == true)
        {
            try
            {
                File.WriteAllBytes(dialog.FileName, this.Model.Character.BinaryData);
            }
            catch (System.Exception ex)
            {
                throw new ClientException("Can't Save File: " + ex.Message, ex);
            }
        }
    }

    private void Button_Open_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new OpenFileDialog
        {
            Filter = "RQCalc Files|*.rqcalc|All Files (*.*)|*.*"
        };

        if (dialog.ShowDialog() == true)
        {
            try
            {
                this.Model.Character.BinaryData = File.ReadAllBytes(dialog.FileName);
            }
            catch (System.Exception ex)
            {
                throw new ClientException("Can't Open File: " + ex.Message, ex);
            }
        }
    }

    private void Button_NewChar_Click(object sender, RoutedEventArgs e)
    {
        if (MessageBox.Show(this, "Создать нового персонажа?", "Создание нового персонажа", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
        {
            this.Model.SetDefaultCharacter();
        }
    }

    private void Button_About_Click(object sender, RoutedEventArgs e)
    {
        var windowModel = new AboutWindowModel(this.Context, this.Model.Version);

        new AboutWindow { Model = windowModel, Owner = this }.ShowDialog();
    }



    private void TextBox_Code_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        var textBox = (TextBox)sender;

        if (e.Key == Key.Enter)
        {
            var be = textBox.GetBindingExpression(TextBox.TextProperty);

            be?.UpdateSource();
        }
    }


    private void Elixir_Click_MouseDown(object sender, MouseButtonEventArgs e)
    {
        var windowModel = new ElixirWindowModel(this.Context, this.Model.Character.Class, this.Model.Character.Elixir);

        new ElixirWindow { Model = windowModel, Owner = this }.SucessDialog(() => this.Model.Character.Elixir = windowModel.Elixir);
    }

    private void Consumables_Click_MouseDown(object sender, MouseButtonEventArgs e)
    {
        var windowModel = new ConsumablesWindowModel(this.Context) { Consumables = this.Model.Character.Consumables };

        new ConsumablesWindow { Model = windowModel, Owner = this }.SucessDialog(() => this.Model.Character.Consumables = windowModel.Consumables);
    }

    private void GuildTalents_Click_MouseDown(object sender, MouseButtonEventArgs e)
    {
        var windowModel = new GuildTalentsWindowModel(this.Context, this.Model.Character);

        new GuildTalentsWindow(this.router) { Model = windowModel, Owner = this }.SucessDialog(() => this.Model.Character.GuildTalents = windowModel.ActiveTalents);
    }

    private void Talents_Click_MouseDown(object sender, MouseButtonEventArgs e)
    {
        var windowModel = new TalentsWindowModel(this.Context, this.Model.Character.GetTemplateEvaluateStats(), this.Model.Character);

        new TalentsWindow(this.router) { Model = windowModel, Owner = this }.SucessDialog(() => this.Model.Character.Talents = windowModel.ActiveTalents.ToList());
    }

    private void Aura_Click_MouseDown(object sender, MouseButtonEventArgs e)
    {
        var windowModel = new AurasWindowModel(this.Context, this.Model.Character.Class, this.Model.Character.Aura, this.Model.Character.Level, this.Model.Character.SharedAuras);

        new AurasWindow { Model = windowModel, Owner = this }.SucessDialog(() =>

            this.Model.Character.UpdateAura(windowModel.Aura, windowModel.SharedAuras.Where(model => model.Active).ToDictionary(model => model.SelectedObject, model => model.WithTalents)));
    }

    private void Buffs_Click_MouseDown(object sender, MouseButtonEventArgs e)
    {
        var windowModel = new BuffsWindowModel(this.Context, this.Model.Character);

        new BuffsWindow { Model = windowModel, Owner = this }.SucessDialog(() => this.Model.Character.Buffs = windowModel.Buffs);
    }


    private void Collections_Click_MouseDown(object sender, MouseButtonEventArgs e)
    {
        var windowModel = new CollectionsWindowModel(this.Context, this.Model.Character) { Items = this.Model.Character.CollectedItems };

        new CollectionsWindow { Model = windowModel, Owner = this }.SucessDialog(() => this.Model.Character.CollectedItems = windowModel.Items);
    }
}