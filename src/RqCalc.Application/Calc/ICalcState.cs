using RqCalc.Domain;

namespace RqCalc.Application.Calc;

public interface ICalcState : ICharacterCalc
{
    IReadOnlyDictionary<IStat, decimal> Stats { get; }
        
    IReadOnlyDictionary<int, decimal> CustomVariables { get; }

    ICalcState ChangeVariable(decimal variable);
}   