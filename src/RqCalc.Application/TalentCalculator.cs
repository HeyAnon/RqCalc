using RqCalc.Application.Settings;
using RqCalc.Domain;
using RqCalc.Domain._Extensions;
using RqCalc.Domain.Talent;
using RqCalc.Model;

namespace RqCalc.Application;

public class TalentCalculator(IVersion lastVersion, ApplicationSettings settings) : ITalentCalculator
{
    public int GetFreeTalents(ITalentBuildSource talentBuildSource)
    {
        var baseAvailableTalents = Math.Min(talentBuildSource.Level, lastVersion.MaxTalentLevel) / settings.TalentLevelMultiplicity;

        var specializationTalents = talentBuildSource.Class.GetSumBonusTalentCount();

        var allAvailableTalents = baseAvailableTalents + specializationTalents;

        var usedTalents = talentBuildSource.Talents.Count;

        return allAvailableTalents - usedTalents;
    }

    public IEnumerable<ITalent> GetLimitedTalents(ITalentBuildSource source)
    {
        var freeTalents = this.GetFreeTalents(source);

        if (freeTalents < 0)
        {
            var removingTalentRequest = from talent in source.Talents

                orderby talent.Branch.Id, talent.HIndex

                select talent;

            var removingTalents = removingTalentRequest.Reverse().Take(-freeTalents).ToList();

            return source.Talents.Except(removingTalents).ToList();
        }
        else
        {
            return source.Talents;
        }
    }
}