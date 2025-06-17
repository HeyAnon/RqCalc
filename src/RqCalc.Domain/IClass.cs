using Framework.Persistent;

using RqCalc.Domain._Base;
using RqCalc.Domain.Talent;

namespace RqCalc.Domain;

public interface IClass : IImageDirectoryBase, IHierarchicalSource<IClass>, IBonusContainer<IClassBonus>
{
    IReadOnlyCollection<IClassLevelHpBonus> LevelHpBonuses { get; }

    IReadOnlyCollection<ITalentBranch> TalentBranches { get; }


    IReadOnlyCollection<IAura> Auras { get; }

    IReadOnlyCollection<IBuff> Buffs { get; }


    IStat PrimaryStat { get; }

    IStat EnergyStat { get; }

    IClassSpecialization Specialization { get; }


    bool AllowExtraWeapon { get; }

    bool IsMelee { get; }

    decimal HpPerVitality { get; }
}