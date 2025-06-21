using System.Collections.ObjectModel;
using Framework.Core;
using RqCalc.Domain._Extensions;
using RqCalc.Domain.CollectedStatistic;
using RqCalc.Model;
using RqCalc.Wpf.Models._Base;

namespace RqCalc.Wpf.Models.Window.Dialog
{
    public class CollectedGroupModel : ContextModel
    {
        public CollectedGroupModel(IServiceProvider context, ICollectedGroup group, ICharacterSource characterSource)
            : base(context)
        {
            if (group == null) throw new ArgumentNullException(nameof(group));
            if (characterSource == null) throw new ArgumentNullException(nameof(characterSource));

            this.Group = @group;
            this.ItemList = this.Group.Items.WhereVersion(this.Context.LastVersion)
                                            .Where(item => item.IsAllowed(characterSource.Gender, this.Context.LastVersion))
                                            .OrderBy(item => item.OrderIndex)
                                            .ToObservableCollection(item => new CollectedItemModel(this.Context) { SelectedObject = item, Activate = true });
        }


        public ICollectedGroup Group { get; }

        public IReadOnlyList<ICollectedItem> Items
        {
            get { return this.ItemList.Where(model => model.Active).ToList(model => model.SelectedObject); }
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
            get { return this.GetValue(v => v.ItemList); }
            private set { this.SetValue(v => v.ItemList, value); }
        }
    }
}