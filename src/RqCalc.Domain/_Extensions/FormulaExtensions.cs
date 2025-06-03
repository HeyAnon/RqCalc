using RqCalc.Domain.Formula;

namespace RqCalc.Domain._Extensions;

public static class FormulaExtensions
{
    public static bool IsEnabled(this IFormula formula)
    {
        return formula != null && formula.Enabled;
    }
}