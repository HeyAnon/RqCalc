using RqCalc.Domain._Base;
using RqCalc.Domain.Equipment;

namespace RqCalc.Domain.BonusType;

public interface IBonusTypeStatCondition : IPersistentIdentityDomainObjectBase
{
    IClass? Class { get; }

    IBonusTypeStat BonusTypeStat { get; }

    IEvent? Event { get; }
        
    IState? State { get; }

    IEquipmentType? EquipmentType { get; }

    bool? IsMaxLevel { get; }

    bool? PairEquipment { get; }

    bool? LostControl { get; }
}