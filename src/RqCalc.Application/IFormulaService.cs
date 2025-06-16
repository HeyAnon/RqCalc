using RqCalc.Application.Calculation;
using RqCalc.Domain.Formula;

namespace RqCalc.Application;

public interface IFormulaService
{
    Func<ICharacterCalculationChangedState, decimal> GetFunc(IFormula formula);
}