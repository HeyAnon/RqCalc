using RqCalc.Domain._Base;
using RqCalc.Domain.Equipment;

namespace RqCalc.Domain.Card;

public interface ICardBonusVariableCondition : IPersistentDomainObjectBase
{
    ICardBonusVariable CardBonusVariable { get; }

    IEquipmentType? EquipmentType { get; }

    bool? IsSingleHandWeapon { get; }
}