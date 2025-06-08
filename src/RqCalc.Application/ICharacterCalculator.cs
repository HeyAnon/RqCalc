using RqCalc.Model;

namespace RqCalc.Application;

public interface ICharacterCalculator
{
    int GetFreeStats(ICharacterSource characterInput);

    ICharacterSource GetDefaultCharacter();
}