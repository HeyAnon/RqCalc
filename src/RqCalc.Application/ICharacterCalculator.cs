using RqCalc.Model;

namespace RqCalc.Application;

public interface ICharacterCalculator
{
    //ISerializer<byte[], ICharacterSource> CharacterBinarySerializer { get; }

    //ISerializer<string, ICharacterSource> CharacterSerializer { get; }

    int GetFreeStats(ICharacterSource characterInput);
    
    CharacterCalculationResult Calculate(ICharacterSource character);
}