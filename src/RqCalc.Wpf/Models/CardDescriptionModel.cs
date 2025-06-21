using System.Collections.ObjectModel;

using Framework.Core;
using Framework.Reactive;
using RqCalc.Domain;
using RqCalc.Domain.Card;
using RqCalc.Model._Extensions;
using RqCalc.Wpf.Models._Base;

namespace RqCalc.Wpf.Models
{
    public class CardDescriptionModel : ContextModel
    {
        public CardDescriptionModel(IServiceProvider context, IReadOnlyDictionary<TextTemplateVariableType, decimal> evaluateStats, ICard card)
            : base(context)
        {
            if (evaluateStats == null) throw new ArgumentNullException(nameof(evaluateStats));
            if (card == null) throw new ArgumentNullException(nameof(card));

            this.Card = card;
            this.BuffDescriptions = card.BuffDescriptions.ToObservableCollection(buff => new TextTemplateModel(this.Context, evaluateStats, buff.ToTextTemplate()));
        }


        public ICard Card
        {
            get { return this.GetValue(v => v.Card); }
            private set { this.SetValue(v => v.Card, value); }
        }


        public ObservableCollection<TextTemplateModel> BuffDescriptions
        {
            get { return this.GetValue(v => v.BuffDescriptions); }
            private set { this.SetValue(v => v.BuffDescriptions, value); }
        }
    }
}