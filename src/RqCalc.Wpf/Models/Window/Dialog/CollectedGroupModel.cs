using Framework.Core;
using Framework.Reactive;

using RqCalc.Domain;
using RqCalc.Domain._Extensions;
using RqCalc.Domain.CollectedStatistic;
using RqCalc.Model;

namespace RqCalc.Wpf.Models.Window.Dialog;

public class CollectedGroupModel : NotifyModelBase
{
    public CollectedGroupModel(IVersion lastVersion, ICollectedGroup group, ICharacterSource characterSource)
    {
        this.Group = @group;
        this.ItemList = this.Group.Items.WhereVersion(lastVersion)
                            .Where(item => item.IsAllowed(characterSource.Gender, lastVersion))
                            .OrderBy(item => item.OrderIndex)
                            .ToObservableCollection(item => new CollectedItemModel { SelectedObject = item, Activate = true });
    }


    public ICollectedGroup Group { get; }

    public IReadOnlyList<ICollectedItem> Items
    {
        get => this.ItemList.Where(model => model.Active).ToList(model => model.SelectedObject);
        set
        {
            foreach (var model in this.ItemList)
            {
                model.Active = value.Contains(model.SelectedObject);
            }
        }
    }

    public ObservableCollection<CollectedItemModel> ItemList
    {
        get => this.GetValue(v => v.ItemList);
        private set => this.SetValue(v => v.ItemList, value);
    }
}