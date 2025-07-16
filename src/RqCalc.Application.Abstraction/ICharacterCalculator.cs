using RqCalc.Model;

namespace RqCalc.Application;

public interface ICharacterCalculator
{
    CharacterCalculationResult Calculate(ICharacterSource character);
}