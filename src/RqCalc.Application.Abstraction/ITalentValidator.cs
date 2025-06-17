using RqCalc.Model;

namespace RqCalc.Application;

public interface ITalentValidator
{
    void Validate(ITalentBuildSource character);
}