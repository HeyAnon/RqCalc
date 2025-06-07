using RqCalc.Model;

namespace RqCalc.Application;

public class CharacterCalculator(ApplicationSettings settings) : ICharacterCalculator
{
    public int GetFreeStats(ICharacterSource characterInput)
    {
        if (characterInput == null) throw new ArgumentNullException(nameof(characterInput));

        var allAvailableStats = (characterInput.Level - 1) * settings.StatsPerLevel;

        var usedStats = characterInput.EditStats.Values.Sum(v => v - 1);

        return allAvailableStats - usedStats;
    }
}