using Framework.Core;
using Framework.DataBase;
using Framework.Reactive;
using Framework.Reactive.ObservableRecurse;

using RqCalc.Domain;
using RqCalc.Domain._Base;
using RqCalc.Domain.CollectedStatistic;
using RqCalc.Model;
using RqCalc.Wpf.Models.Window.Dialog._Base;

namespace RqCalc.Wpf.Models.Window.Dialog;

public class CollectionsWindowModel : NotifyModelBase, IMultiSelectModel
{
    public CollectionsWindowModel(IVersion lastVersion, IDataSource<IPersistentDomainObjectBase> dataSource, ICharacterSource characterSource)
    {
        this.Groups = dataSource.GetFullList<ICollectedGroup>().OrderBy(g => g.OrderIndex)
                                .ToObservableCollection(g => new CollectedGroupModel(lastVersion, g, characterSource));

        this.SubscribeExplicit(rootRule => rootRule.SelectMany(rootModel => rootModel.Groups,

            groupRule => groupRule.SelectMany(group => group.ItemList, item => item.Subscribe(itemRule => itemRule.Active, this.RecalculateTotalSelected))));

        this.RecalculateTotalSelected();

        this.Items = characterSource.CollectedItems;
    }


    public IReadOnlyList<ICollectedItem> Items
    {
        get => this.Groups.SelectMany(g => g.Items).ToList();
        set
        {
            var request = from groupModel in this.Groups

                join item in value on groupModel.Group equals item.Group into items

                select new
                {
                    GroupModel = groupModel,

                    Items = items.ToList()
                };

            foreach (var pair in request)
            {
                pair.GroupModel.Items = pair.Items;
            }
        }
    }

    public ObservableCollection<CollectedGroupModel> Groups
    {
        get => this.GetValue(v => v.Groups);
        private set => this.SetValue(v => v.Groups, value);
    }

    public bool? TotalSelected
    {
        get => this.GetValue(v => v.TotalSelected);
        set
        {
            if (value != null)
            {
                this.Groups.SelectMany(g => g.ItemList).Foreach(item => item.Active = value.Value);

                this.RecalculateTotalSelected();
            }
        }
    }

    private void RecalculateTotalSelected() => this.SetValue(v => v.TotalSelected, this.GetTotalSelectedState());

    private bool? GetTotalSelectedState() =>
        this.Groups.SelectMany(g => g.ItemList).All(item => item.Active)  ? true
        : this.Groups.SelectMany(g => g.ItemList).All(item => !item.Active) ? false : null;
}