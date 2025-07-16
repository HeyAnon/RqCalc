using RqCalc.Domain.Talent;

namespace RqCalc.Model;

public interface ITalentBuildSource : IClassBuildSource
{
    IReadOnlyList<ITalent> Talents { get; }
}