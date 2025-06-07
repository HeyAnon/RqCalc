using RqCalc.Domain;

namespace RqCalc.Application.Calc;

internal interface ICalcState : ICharacterCalc
{
    IReadOnlyDictionary<IStat, decimal> Stats { get; }
        
    IReadOnlyDictionary<int, decimal> CustomVariables { get; }

    ICalcState ChangeVariable(decimal variable);
}