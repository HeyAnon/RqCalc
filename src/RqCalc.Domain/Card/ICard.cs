using Framework.Persistent;
using RqCalc.Core;
using RqCalc.Domain._Base;
using RqCalc.Domain.Equipment;

namespace RqCalc.Domain.Card;

public interface ICard : IDirectoryBase, ITypeObject<ICardType>, IBonusContainer<ICardBonus>, IVersionObject, ILegacyObject
{
    IEnumerable<IBuff> Buffs { get; }

    IEnumerable<ICardEquipmentSlot> EquipmentSlots { get; }

    IEnumerable<ICardEquipmentType> EquipmentTypes { get; }

    IEnumerable<ICardBuffDescription> BuffDescriptions { get; }

    CardGroupInfo Group { get; }

    bool? IsSingleHandWeapon { get; }

    bool? IsMeleeWeapon { get; }

    IEquipmentClass MinEquipmentClass { get; }

    ICardSet Set { get; }
}