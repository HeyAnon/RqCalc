using RqCalc.Model;

namespace RqCalc.Application;

public interface ICharacterCalculator
{
    //ISerializer<byte[], ICharacterSource> CharacterBinarySerializer { get; }

    //ISerializer<string, ICharacterSource> CharacterSerializer { get; }

    
    CharacterCalculationResult Calculate(ICharacterSource character);
}