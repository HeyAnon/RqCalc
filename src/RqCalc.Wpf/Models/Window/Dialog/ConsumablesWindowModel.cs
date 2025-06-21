using System.Collections.ObjectModel;
using Framework.Core;
using Framework.Reactive;
using Framework.Reactive.ObservableRecurse;
using RqCalc.Domain;
using RqCalc.Wpf.Models._Base;
using RqCalc.Wpf.Models.Window.Dialog._Base;

namespace RqCalc.Wpf.Models.Window.Dialog
{
    public class ConsumablesWindowModel : ContextModel, IMultiSelectModel
    {
        public ConsumablesWindowModel(IServiceProvider context)
            : base(context)
        {
            this.ConsumableList = this.Context.DataSource.GetFullList<IConsumable>().ToObservableCollection(consumable => new ConsumableModel (this.Context) { SelectedObject = consumable, Activate = true });

            this.SubscribeExplicit(rootRule => rootRule.SelectMany(rootModel => rootModel.ConsumableList, item => item.Subscribe(itemRule => itemRule.Active, this.RecalcTotalSelected)));

            this.RecalcTotalSelected();
        }


        public IReadOnlyList<IConsumable> Consumables
        {
            get { return this.ConsumableList.Where(model => model.Active).ToList(model => model.SelectedObject); }
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
            get { return this.GetValue(v => v.ConsumableList); }
            private set { this.SetValue(v => v.ConsumableList, value); }
        }

        public bool? TotalSelected
        {
            get
            {
                return this.GetValue(v => v.TotalSelected);
            }
            set
            {
                if (value != null)
                {
                    this.ConsumableList.Foreach(item => item.Active = value.Value);

                    this.RecalcTotalSelected();
                }
            }
        }

        private void RecalcTotalSelected()
        {
            this.SetValue(v => v.TotalSelected, this.GetTotalSelectedState());
        }

        private bool? GetTotalSelectedState()
        {
            return this.ConsumableList.All(item => item.Active) ? (bool?)true
                 : this.ConsumableList.All(item => !item.Active) ? (bool?)false : null;
        }
    }
}