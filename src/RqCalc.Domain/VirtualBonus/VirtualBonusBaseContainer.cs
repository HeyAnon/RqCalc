using RqCalc.Domain._Base;

namespace RqCalc.Domain.VirtualBonus;

public record VirtualBonusBaseContainer(IReadOnlyList<IBonusBase> Bonuses) : IBonusContainer<IBonusBase>;