using Framework.Core;
using Framework.DataBase;
using Framework.Reactive;

using RqCalc.Domain;
using RqCalc.Domain._Base;
using RqCalc.Domain._Extensions;
using RqCalc.Model;

namespace RqCalc.Wpf.Models.Window;

public class MainWindowModel : NotifyModelBase
{
    private readonly ApplicationSettings settings;

    private readonly IModelFactory modelFactory;

    public MainWindowModel(
        IModelFactory modelFactory,
        WpfApplicationSettings wpfApplicationSettings,
        ApplicationSettings settings,
        IDataSource<IPersistentDomainObjectBase> dataSource,
        IVersion lastVersion)
    {
        this.settings = settings;
        this.modelFactory = modelFactory;
        this.Version = wpfApplicationSettings.Version;

        this.Genders = dataSource.GetFullList<IGender>().ToObservableCollection();
        this.Classes = dataSource.GetFullList<IClass>().ToObservableCollection();
        this.Events = dataSource.GetFullList<IEvent>().WhereVersion(lastVersion).ToObservableCollection();
        this.States = dataSource.GetFullList<IState>().ToObservableCollection();

        this.SetDefaultCharacter();
    }

    public Version Version { get; }

    public string TitleInfo => $"GUI: {this.Version} | DatabaseUpdate: {this.settings.UpdateDate:dd.MM.yyyy}";

    public ObservableCollection<IGender> Genders
    {
        get => this.GetValue(v => v.Genders);
        private set => this.SetValue(v => v.Genders, value);
    }

    public ObservableCollection<IClass> Classes
    {
        get => this.GetValue(v => v.Classes);
        private set => this.SetValue(v => v.Classes, value);
    }

    public ObservableCollection<IEvent> Events
    {
        get => this.GetValue(v => v.Events);
        private set => this.SetValue(v => v.Events, value);
    }

    public ObservableCollection<IState> States
    {
        get => this.GetValue(v => v.States);
        private set => this.SetValue(v => v.States, value);
    }

    public CharacterChangeModel Character
    {
        get => this.GetValue(v => v.Character);
        set => this.SetValue(v => v.Character, value);
    }


    public void SetDefaultCharacter() => this.Character = this.modelFactory.CreateCharacterChangeModel(null);
}