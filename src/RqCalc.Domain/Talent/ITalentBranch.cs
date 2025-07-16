using RqCalc.Domain._Base;

namespace RqCalc.Domain.Talent;

public interface ITalentBranch : IDirectoryBase
{
    IClass Class { get; }

    IReadOnlyCollection<ITalent> Talents { get; }
}