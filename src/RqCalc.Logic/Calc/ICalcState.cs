using RqCalc.Domain.Persistent;

namespace RqCalc.Logic.Calc;

internal interface ICalcState : ICharacterCalc
{
    IReadOnlyDictionary<IStat, decimal> Stats { get; }
        
    IReadOnlyDictionary<int, decimal> CustomVariables { get; }

    ICalcState ChangeVariable(decimal variable);
}