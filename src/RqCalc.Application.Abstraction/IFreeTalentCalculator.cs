using RqCalc.Model;

namespace RqCalc.Application;

public interface IFreeTalentCalculator
{
    int GetFreeTalents(ITalentBuildSource characterInput);
}