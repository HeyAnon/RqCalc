using RqCalc.Domain.Formula;

namespace RqCalc.Domain._Extensions;

public static class FormulaExtensions
{
    public static bool IsEnabled(this IFormula formula) => formula != null && formula.Enabled;
}