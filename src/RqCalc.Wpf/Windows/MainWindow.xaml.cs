using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using Microsoft.Win32;

using RqCalc.Wpf.Exception;
using RqCalc.Wpf.Models;
using RqCalc.Wpf.Models.Window;
using RqCalc.Wpf.Windows.Dialog;
using RqCalc.Wpf.Windows.Dialog._Base;
using RqCalc.Wpf.Windows.Route;

namespace RqCalc.Wpf.Windows;

public partial class MainWindow
{
    private readonly IModelFactory modelFactory;

    private readonly ICodeRouter codeRouter;

    protected MainWindow() => this.InitializeComponent();

    public MainWindow(WpfApplicationSettings wpfApplicationSettings, IModelFactory modelFactory, ICodeRouter codeRouter)
        : this()
    {
        this.modelFactory = modelFactory;
        this.codeRouter = codeRouter;
        this.DataContext = modelFactory.CreateMainWindowModel();

        if (wpfApplicationSettings.StartupCode != null)
        {
            this.Model.Character = modelFactory.CreateCharacterChangeModel(wpfApplicationSettings.StartupCode);
        }
    }


    private MainWindowModel Model => (MainWindowModel)this.DataContext;


    private void Button_Link_Click(object sender, RoutedEventArgs e) => this.codeRouter.RouteMain(this.Model.Character.Code);

    private void Button_Save_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new SaveFileDialog { Filter = "RQCalc Files|*.rqcalc|All Files (*.*)|*.*" };

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
        var dialog = new OpenFileDialog { Filter = "RQCalc Files|*.rqcalc|All Files (*.*)|*.*" };

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
        if (MessageBox.Show(
                this,
                "Создать нового персонажа?",
                "Создание нового персонажа",
                MessageBoxButton.OKCancel,
                MessageBoxImage.Question)
            == MessageBoxResult.OK)
        {
            this.Model.SetDefaultCharacter();
        }
    }

    private void Button_About_Click(object sender, RoutedEventArgs e)
    {
        var windowModel = this.modelFactory.CreateAboutWindowModel();

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
        var windowModel = this.Model.Character.CreateElixirWindowModel();

        new ElixirWindow { Model = windowModel, Owner = this }.SuccessDialog(() => this.Model.Character.Elixir = windowModel.Elixir);
    }

    private void Consumables_Click_MouseDown(object sender, MouseButtonEventArgs e)
    {
        var windowModel = this.Model.Character.CreateConsumablesWindowModel();

        new ConsumablesWindow { Model = windowModel, Owner = this }.SuccessDialog(() => this.Model.Character.Consumables =
                                                                                            windowModel.Consumables);
    }

    private void GuildTalents_Click_MouseDown(object sender, MouseButtonEventArgs e)
    {
        var windowModel = this.Model.Character.CreateGuildTalentsWindowModel();

        new GuildTalentsWindow(this.codeRouter) { Model = windowModel, Owner = this }.SuccessDialog(() =>
            this.Model.Character.GuildTalents = windowModel.ActiveTalents);
    }

    private void Talents_Click_MouseDown(object sender, MouseButtonEventArgs e)
    {
        var windowModel = this.Model.Character.CreateTalentsWindowModel();

        new TalentsWindow(this.codeRouter) { Model = windowModel, Owner = this }.SuccessDialog(() => this.Model.Character.Talents =
            windowModel.ActiveTalents.ToList());
    }

    private void Aura_Click_MouseDown(object sender, MouseButtonEventArgs e)
    {
        var windowModel = this.Model.Character.CreateAurasWindowModel();

        new AurasWindow { Model = windowModel, Owner = this }.SuccessDialog(() => this.Model.Character.UpdateAura(
                                                                                windowModel.Aura,
                                                                                windowModel.SharedAuras.Where(model => model.Active)
                                                                                           .ToDictionary(
                                                                                               model => model.SelectedObject,
                                                                                               model => model.WithTalents)));
    }

    private void Buffs_Click_MouseDown(object sender, MouseButtonEventArgs e)
    {
        var windowModel = this.Model.Character.CreateBuffsWindowModel();

        new BuffsWindow { Model = windowModel, Owner = this }.SuccessDialog(() => this.Model.Character.Buffs = windowModel.Buffs);
    }


    private void Collections_Click_MouseDown(object sender, MouseButtonEventArgs e)
    {
        var windowModel = this.Model.Character.CreateCollectionsWindowModel();

        new CollectionsWindow { Model = windowModel, Owner = this }.SuccessDialog(() => this.Model.Character.CollectedItems =
                                                                                            windowModel.Items);
    }
}
