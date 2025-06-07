using RqCalc.Model;

namespace RqCalc.Application;

public interface ITalentCalculator
{
    int GetFreeTalents(ITalentBuildSource characterInput);
}