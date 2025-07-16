using RqCalc.Domain.Card;
using RqCalc.Domain.Equipment;
using RqCalc.Domain.Stamp;

namespace RqCalc.Model.Impl;

public class CharacterEquipmentData : ICharacterEquipmentData, IEquatable<CharacterEquipmentData>
{
    public int Upgrade
    {
        get;
        set;
    }

    public required IEquipment Equipment
    {
        get;
        set;
    }

    public List<ICard?> Cards
    {
        get;
        set;
    } = [];

    public IStampVariant? StampVariant
    {
        get;
        set;
    }

    public bool Active
    {
        get;
        set;
    } = true;

    public IEquipmentElixir? Elixir
    {
        get;
        set;
    }



    IReadOnlyList<ICard?> ICharacterEquipmentData.Cards => this.Cards;

    public override int GetHashCode() => this.Equipment.Id ^ this.Upgrade ^ this.Cards.Count ^ this.Active.GetHashCode();

    public override bool Equals(object? obj) => this.Equals(obj as CharacterEquipmentData);

    public bool Equals(CharacterEquipmentData? other)
    {
        if (other is null) return false;
            
        var cards = Enumerable.Reverse(this.Cards).SkipWhile(v => v == null).Reverse();

        var otherCards = Enumerable.Reverse(other.Cards).SkipWhile(v => v == null).Reverse();

        return this.Upgrade == other.Upgrade 
               && this.Active == other.Active
               && this.Equipment == other.Equipment
               && this.StampVariant == other.StampVariant
               && this.Elixir == other.Elixir
               && cards.SequenceEqual(otherCards);
    }
}
