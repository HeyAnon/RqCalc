using Framework.Core;
using Framework.DataBase;
using Framework.Reactive;
using Framework.Reactive.ObservableRecurse;

using RqCalc.Domain;
using RqCalc.Domain._Base;
using RqCalc.Wpf.Models.Window.Dialog._Base;

namespace RqCalc.Wpf.Models.Window.Dialog;

public class ConsumablesWindowModel : NotifyModelBase, IMultiSelectModel
{
    public ConsumablesWindowModel(IDataSource<IPersistentDomainObjectBase> dataSource)
    {
        this.ConsumableList = dataSource.GetFullList<IConsumable>().ToObservableCollection(consumable => new ConsumableModel { SelectedObject = consumable, Activate = true });

        this.SubscribeExplicit(rootRule => rootRule.SelectMany(rootModel => rootModel.ConsumableList, item => item.Subscribe(itemRule => itemRule.Active, this.RecalcTotalSelected)));

        this.RecalcTotalSelected();
    }


    public IReadOnlyList<IConsumable> Consumables
    {
        get => this.ConsumableList.Where(model => model.Active).ToList(model => model.SelectedObject);
        set
        {
            foreach (var model in this.ConsumableList)
            {
                model.Active = value.Contains(model.SelectedObject);
            }
        }
    }

    public ObservableCollection<ConsumableModel> ConsumableList
    {
        get => this.GetValue(v => v.ConsumableList);
        private set => this.SetValue(v => v.ConsumableList, value);
    }

    public bool? TotalSelected
    {
        get => this.GetValue(v => v.TotalSelected);
        set
        {
            if (value != null)
            {
                this.ConsumableList.Foreach(item => item.Active = value.Value);

                this.RecalcTotalSelected();
            }
        }
    }

    private void RecalcTotalSelected() => this.SetValue(v => v.TotalSelected, this.GetTotalSelectedState());

    private bool? GetTotalSelectedState() =>
        this.ConsumableList.All(item => item.Active) ? (bool?)true
        : this.ConsumableList.All(item => !item.Active) ? (bool?)false : null;
}