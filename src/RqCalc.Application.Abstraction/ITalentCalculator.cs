using RqCalc.Domain.Talent;
using RqCalc.Model;

namespace RqCalc.Application;

public interface ITalentCalculator
{
    IEnumerable<ITalent> GetLimitedTalents(ITalentBuildSource source);
}