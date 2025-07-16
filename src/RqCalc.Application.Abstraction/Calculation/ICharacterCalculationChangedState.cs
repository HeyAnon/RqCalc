using RqCalc.Domain;

namespace RqCalc.Application.Calculation;

public interface ICharacterCalculationChangedState : ICharacterCalculationState
{
    IReadOnlyDictionary<IStat, decimal> Stats { get; }
        
    IReadOnlyDictionary<int, decimal> CustomVariables { get; }

    ICharacterCalculationChangedState ChangeVariable(decimal variable);
}   