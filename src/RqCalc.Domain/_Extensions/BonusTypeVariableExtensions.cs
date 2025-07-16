using RqCalc.Domain.BonusType;

namespace RqCalc.Domain._Extensions;

public static class BonusTypeVariableExtensions
{
    public static bool IsDynamic(this IBonusTypeVariable variable) => variable.MulFormula.IsEnabled();
}