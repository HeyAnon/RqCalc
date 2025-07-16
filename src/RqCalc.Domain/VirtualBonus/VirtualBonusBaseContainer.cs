using RqCalc.Domain._Base;

namespace RqCalc.Domain.VirtualBonus;

public record VirtualBonusBaseContainer(IReadOnlyCollection<IBonusBase> Bonuses) : IBonusContainer<IBonusBase>
{
    public VirtualBonusBaseContainer(IEnumerable<IBonusBase> bonuses) : this(bonuses.ToList())
    {
    }
}