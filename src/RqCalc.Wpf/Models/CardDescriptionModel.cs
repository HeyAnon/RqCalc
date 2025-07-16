using Framework.Core;
using Framework.Reactive;

using RqCalc.Domain;
using RqCalc.Domain.Card;
using RqCalc.Model._Extensions;

namespace RqCalc.Wpf.Models;

public class CardDescriptionModel : NotifyModelBase
{
    public CardDescriptionModel(IReadOnlyDictionary<TextTemplateVariableType, decimal> evaluateStats, ICard card)
    {
        this.Card = card;
        this.BuffDescriptions = card.BuffDescriptions.ToObservableCollection(buff => new TextTemplateModel(evaluateStats, buff.ToTextTemplate()));
    }


    public ICard Card
    {
        get => this.GetValue(v => v.Card);
        private set => this.SetValue(v => v.Card, value);
    }


    public ObservableCollection<TextTemplateModel> BuffDescriptions
    {
        get => this.GetValue(v => v.BuffDescriptions);
        private set => this.SetValue(v => v.BuffDescriptions, value);
    }
}