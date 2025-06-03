using RqCalc.Domain.Persistent._Base._Blocks;
using RqCalc.Domain.Persistent.Talent;

namespace RqCalc.Domain.Persistent;

public interface IAura : IImageDirectoryBase, IBonusContainer<IAuraBonus>, IClassObject, ILevelObject, IVersionObject
{
    IEnumerable<ITalentBonus> DependencyTalentBonuses { get; }
}