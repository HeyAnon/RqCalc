using RqCalc.Domain.Talent;
using RqCalc.Model;

namespace RqCalc.Application;

public interface ITalentCalculator
{
    //ISerializer<string, ITalentBuildSource> TalentSerializer { get; }

    int GetFreeTalents(ITalentBuildSource characterInput);

    IEnumerable<ITalent> GetLimitedTalents(ITalentBuildSource source);
}