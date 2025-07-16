using RqCalc.Domain._Base;

namespace RqCalc.Domain.VirtualBonus;

public record VirtualBonusVariable(int Index, int Value) : IBonusVariable;