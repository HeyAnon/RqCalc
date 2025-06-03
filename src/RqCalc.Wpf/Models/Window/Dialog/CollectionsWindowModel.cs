using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using Anon.RQ_Calc.Domain;
using Anon.RQ_Calc.Logic;

using Framework.Core;

using Framework.Reactive;
using Framework.Reactive.ObservableRecurse;

namespace Anon.RQ_Calc.WPF
{
    public class CollectionsWindowModel : ContextModel, IMultiSelectModel
    {
        public CollectionsWindowModel(IApplicationContext context, ICharacterSource characterSource)
            : base(context)
        {
            if (characterSource == null) throw new ArgumentNullException(nameof(characterSource));

            this.Groups = this.Context.DataSource.GetFullList<ICollectedGroup>().OrderBy(g => g.OrderIndex)
                                                 .ToObservableCollection(g => new CollectedGroupModel(this.Context, g, characterSource));

            this.SubscribeExplicit(rootRule => rootRule.SelectMany(rootModel => rootModel.Groups,
            
                groupRule => groupRule.SelectMany(group => group.ItemList, item => item.Subscribe(itemRule => itemRule.Active, this.RecalcTotalSelected))));

            this.RecalcTotalSelected();
        }


        public IReadOnlyList<ICollectedItem> Items
        {
            get { return this.Groups.SelectMany(g => g.Items).ToList(); }
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
            get { return this.GetValue(v => v.Groups); }
            private set { this.SetValue(v => v.Groups, value); }
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
                    this.Groups.SelectMany(g => g.ItemList).Foreach(item => item.Active = value.Value);

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
            return this.Groups.SelectMany(g => g.ItemList).All(item => item.Active)  ? (bool?)true
                 : this.Groups.SelectMany(g => g.ItemList).All(item => !item.Active) ? (bool?)false : null;
        }
    }
}