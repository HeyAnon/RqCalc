using RqCalc.Domain;

namespace RqCalc.Application.Calc;

public interface ICharacterCalculationState : ICharacterCalculationStateBase
{
    IReadOnlyDictionary<IStat, decimal> Stats { get; }
        
    IReadOnlyDictionary<int, decimal> CustomVariables { get; }

    ICharacterCalculationState ChangeVariable(decimal variable);
}   