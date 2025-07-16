using Framework.Core;
using Framework.Reactive;
using Framework.Reactive.ObservableRecurse;

using RqCalc.Domain.Card;

namespace RqCalc.Wpf.Models.Window.Dialog;

public interface ICardGroup
{
    ICard ActiveCard { get; }

    string Name { get; }

    bool IsLegacy { get; }

    IEnumerable<ICard> GetAvailableCards();

    void RefreshSource();
}

public static class CardGroup
{
    public static IEnumerable<ICardGroup> Create(IEnumerable<ICard> cards) =>
        from card in cards

        group card by card.Group?.Key ?? Guid.NewGuid().ToString() into g

        select g.Count() == 1 ? (ICardGroup)new SingleCardGroup(g.Single())

                   : new MultiCardGroup(g.Key, g);
}


public class SingleCardGroup(ICard card) : ICardGroup
{
    public ICard ActiveCard { get; set; } = card;

    public string Name => this.ActiveCard.Name;

    public bool IsLegacy => this.ActiveCard.IsLegacy;


    public IEnumerable<ICard> GetAvailableCards()
    {
        yield return this.ActiveCard;
    }

    public void RefreshSource()
    {
            
    }
}


public class MultiCardGroup : NotifyModelBase, ICardGroup
{
    private readonly ICard[] baseCards;


    public MultiCardGroup(string name, IEnumerable<ICard> cards)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));
        if (cards == null) throw new ArgumentNullException(nameof(cards));

        this.Name = name;

        this.baseCards = cards.OrderBy(card => card.Group.OrderKey).ToArray();

        this.RefreshSource();

        this.SubscribeExplicit(rule => rule.Subscribe(v => v.ShowLegacy, this.RefreshSource));
    }


    public string Name { get; }

    public bool IsLegacy => this.baseCards.All(bc => bc.IsLegacy);

    public ICard? ActiveCard
    {
        get => this.GetValue(v => v.ActiveCard);
        set => this.SetValue(v => v.ActiveCard, value);
    }
        
    public bool ShowLegacy
    {
        get => this.GetValue(v => v.ShowLegacy);
        set => this.SetValue(v => v.ShowLegacy, value);
    }

    public ObservableCollection<ICard> Cards
    {
        get => this.GetValue(v => v.Cards);
        private set => this.SetValue(v => v.Cards, value);
    }


    public IEnumerable<ICard> GetAvailableCards() => this.baseCards;

    public void RefreshSource()
    {
        this.Cards = this.baseCards.Where(card => !card.IsLegacy || this.ShowLegacy).ToObservableCollection();

        if (this.ActiveCard == null)
        {
            this.ActiveCard = this.Cards.LastOrDefault();
        }
    }
}