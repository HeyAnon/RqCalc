using RqCalc.Domain.Persistent._Base;
using RqCalc.Domain.Persistent._Base._Blocks;

namespace RqCalc.Domain.Persistent.Talent;

public interface ITalentBranch : IDirectoryBase, IClassObject
{
    IEnumerable<ITalent> Talents { get; }
}