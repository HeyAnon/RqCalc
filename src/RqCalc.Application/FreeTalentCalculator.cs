using RqCalc.Domain;
using RqCalc.Domain._Extensions;
using RqCalc.Model;

namespace RqCalc.Application;

public class FreeTalentCalculator(IVersion lastVersion, ApplicationSettings settings) : IFreeTalentCalculator
{
    public int GetFreeTalents(ITalentBuildSource talentBuildSource)
    {
        var baseAvailableTalents = Math.Min(talentBuildSource.Level, lastVersion.MaxTalentLevel) / settings.TalentLevelMultiplicity;

        var specializationTalents = talentBuildSource.Class.GetSumBonusTalentCount();

        var allAvailableTalents = baseAvailableTalents + specializationTalents;

        var usedTalents = talentBuildSource.Talents.Count;

        return allAvailableTalents - usedTalents;
    }
}