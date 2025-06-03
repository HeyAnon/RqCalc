using Framework.Persistent;

using RqCalc.Domain.Persistent._Base._Blocks;
using RqCalc.Domain.Persistent.Talent;

namespace RqCalc.Domain.Persistent;

public interface IClass : IImageDirectoryBase, IHierarchicalSource<IClass>, IBonusContainer<IClassBonus>
{
    IEnumerable<IClassLevelHpBonus> LevelHpBonuses { get; }

    IEnumerable<ITalentBranch> TalentBranches { get; }


    IEnumerable<IAura> Auras { get; }

    IEnumerable<IBuff> Buffs { get; }


    IStat PrimaryStat { get; }

    IStat EnergyStat { get; }

    IClassSpecialization Specialization { get; }


    bool AllowExtraWeapon { get; }

    bool IsMelee { get; }

    decimal HpPerVitality { get; }
}