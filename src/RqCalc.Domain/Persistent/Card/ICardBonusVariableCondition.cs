using RqCalc.Domain.Persistent._Base;
using RqCalc.Domain.Persistent.Equipment;

namespace RqCalc.Domain.Persistent.Card;

public interface ICardBonusVariableCondition : IPersistentDomainObjectBase
{
    ICardBonusVariable CardBonusVariable { get; }

    IEquipmentType? EquipmentType { get; }

    bool? IsSingleHandWeapon { get; }
}