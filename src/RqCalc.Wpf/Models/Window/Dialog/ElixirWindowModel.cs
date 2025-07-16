using Framework.Core;
using Framework.DataBase;
using Framework.Reactive;
using Framework.Reactive.ObservableRecurse;

using RqCalc.Domain;
using RqCalc.Domain._Base;
using RqCalc.Domain._Extensions;
using RqCalc.Wpf.Models.Window.Dialog._Base;

namespace RqCalc.Wpf.Models.Window.Dialog;

public class ElixirWindowModel : NotifyModelBase, ILegacyModel, IClearModel
{
    private readonly IDataSource<IPersistentDomainObjectBase> dataSource;

    private readonly IClass currentClass;

    public ElixirWindowModel(IDataSource<IPersistentDomainObjectBase> dataSource, IClass currentClass, IElixir? startupElixir)
    {
        this.dataSource = dataSource;
        this.currentClass = currentClass;

        this.Elixir = startupElixir;

        this.ShowLegacy = this.Elixir.Maybe(e => e.IsLegacy);

        this.Refresh();

        this.SubscribeExplicit(rule => rule.Subscribe(v => v.ShowLegacy, this.Refresh));
    }


    public ObservableCollection<IElixir> Elixirs
    {
        get => this.GetValue(v => v.Elixirs);
        private set => this.SetValue(v => v.Elixirs, value);
    }

    public IElixir? Elixir
    {
        get => this.GetValue(v => v.Elixir);
        set => this.SetValue(v => v.Elixir, value);
    }


    public bool ShowLegacy
    {
        get => this.GetValue(v => v.ShowLegacy);
        set => this.SetValue(v => v.ShowLegacy, value);
    }


    public bool HasLegacy { get; } = true;

    public bool CloseDialog { get; } = true;


    public void Clear() => this.Elixir = null;

    private void Refresh()
    {
        var request = from elixir in this.dataSource.GetFullList<IElixir>()

            where !elixir.IsLegacy || this.ShowLegacy

            orderby elixir.GetOrderIndex(this.currentClass) descending

            select elixir;


        this.Elixirs = request.ToObservableCollection();

        this.Elixir ??= this.Elixirs.FirstOrDefault();
    }
}