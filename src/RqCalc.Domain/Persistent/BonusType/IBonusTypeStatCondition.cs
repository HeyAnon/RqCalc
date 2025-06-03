using RqCalc.Domain.Persistent._Base;
using RqCalc.Domain.Persistent._Base._Blocks;
using RqCalc.Domain.Persistent.Equipment;

namespace RqCalc.Domain.Persistent.BonusType;

public interface IBonusTypeStatCondition : IPersistentIdentityDomainObjectBase, IClassObject
{
    IBonusTypeStat BonusTypeStat { get; }

    IEvent Event { get; }
        
    IState State { get; }

    IEquipmentType EquipmentType { get; }

    bool? IsMaxLevel { get; }

    bool? PairEquipment { get; }

    bool? LostControl { get; }
}