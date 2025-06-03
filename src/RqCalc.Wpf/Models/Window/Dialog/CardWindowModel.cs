using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using Framework.Core;

using Framework.Reactive;
using Framework.Reactive.ObservableRecurse;

using Anon.RQ_Calc.Domain;
using Anon.RQ_Calc.Logic;

namespace Anon.RQ_Calc.WPF
{
    public class CardWindowModel : ContextModel, IClearModel, ILegacyModel, IEvaluateClientContext
    {
        private readonly ICardGroup[] _baseGroups;



        public CardWindowModel(IApplicationContext context, IReadOnlyDictionary<TextTemplateVariableType, decimal> evaluateStats, IEquipmentType equipmentType, int cardIndex, IClass currentClass, ICard startupCard, IEquipmentClass equipmentClass)
            : base(context)
        {
            if (equipmentType == null) throw new ArgumentNullException(nameof(equipmentType));
            if (currentClass == null) throw new ArgumentNullException(nameof(currentClass));
            if (equipmentClass == null) throw new ArgumentNullException(nameof(equipmentClass));

            this.EvaluateStats = evaluateStats;


            this._baseGroups = this.Context.DataSource.GetFullList<ICard>()
                                                      .WhereVersion(this.Context.LastVersion)
                                                      .Where(card => card.IsAllowed(equipmentType, this.Context.LastVersion, equipmentClass))
                                                      .Where(card => card.Type.Element == null || cardIndex == 0)
                                                      .OrderBy(card => card.Type.Id)
                                                      .ThenByDescending(card => card.GetOrderIndex(currentClass))
                                                      .Pipe(WPF.CardGroup.Create)
                                                      .ToArray();

            this.ShowLegacy = startupCard.Maybe(e => e.IsLegacy);

            {
                this.CardGroup = startupCard.Maybe(card => this._baseGroups.SingleOrDefault(cardGroup => cardGroup.GetAvailableCards().Contains(card)));

                if (this.CardGroup is MultiCardGroup multiCardGroup)
                {
                    multiCardGroup.ActiveCard = startupCard;
                }
            }
            

            this.RefreshSource(); 
            
            this.SubscribeExplicit(rule => rule.Subscribe(v => v.ShowLegacy, this.RefreshSource));
        }


        public IReadOnlyDictionary<TextTemplateVariableType, decimal> EvaluateStats
        {
            get { return this.GetValue(v => v.EvaluateStats); }
            private set { this.SetValue(v => v.EvaluateStats, value); }
        }

        public ObservableCollection<ICardGroup> CardGroups
        {
            get { return this.GetValue(v => v.CardGroups); }
            private set { this.SetValue(v => v.CardGroups, value); }
        }

        public ICardGroup CardGroup
        {
            get { return this.GetValue(v => v.CardGroup); }
            set { this.SetValue(v => v.CardGroup, value); }
        }

        public ICard Card => this.CardGroup.Maybe(cg => cg.ActiveCard);

        public bool ShowLegacy
        {
            get { return this.GetValue(v => v.ShowLegacy); }
            set { this.SetValue(v => v.ShowLegacy, value); }
        }

        public bool HasLegacy => this._baseGroups.Any(g => g.IsLegacy);

        public bool CloseDialog { get; } = true;


        public void Clear()
        {
            this.CardGroup = null;
        }


        private void RefreshSource()
        {
            this.CardGroups = this._baseGroups.Where(card => !card.IsLegacy || this.ShowLegacy).ToObservableCollection();

            this.CardGroups.OfTypeStrong<ICardGroup, MultiCardGroup>().Foreach(cardGroup => cardGroup.ShowLegacy = this.ShowLegacy);

            if (this.CardGroup == null)
            {
                this.CardGroup = this.CardGroups.FirstOrDefault();
            }
        }
    }
}