using RqCalc.Model;

namespace RqCalc.Application;

public class FreeStatCalculator(ApplicationSettings settings) : IFreeStatCalculator
{
    public int GetFreeStats(ICharacterSource characterInput)
    {
        var allAvailableStats = (characterInput.Level - 1) * settings.StatsPerLevel;

        var usedStats = characterInput.EditStats.Values.Sum(v => v - 1);

        return allAvailableStats - usedStats;
    }
}