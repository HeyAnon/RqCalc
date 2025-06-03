using RqCalc.Domain._Base;
using RqCalc.Domain.Talent;

namespace RqCalc.Domain;

public interface IAura : IImageDirectoryBase, IBonusContainer<IAuraBonus>, ILevelObject, IVersionObject
{
    IClass Class { get; }

    IEnumerable<ITalentBonus> DependencyTalentBonuses { get; }
}