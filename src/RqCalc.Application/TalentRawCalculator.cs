using RqCalc.Domain.Talent;
using RqCalc.Model;

namespace RqCalc.Application;

public class TalentRawCalculator(IFreeTalentCalculator freeTalentCalculator) : ITalentCalculator
{
    public const string Key = "Raw";

    public IEnumerable<ITalent> GetLimitedTalents(ITalentBuildSource source)
    {
        var freeTalents = freeTalentCalculator.GetFreeTalents(source);

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
