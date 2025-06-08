using RqCalc.Application.Calc;
using RqCalc.Domain.Formula;

namespace RqCalc.Application;

public interface IFormulaService
{
    Func<ICalcState, decimal> GetFunc(IFormula formula);
}