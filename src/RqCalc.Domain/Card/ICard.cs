using Framework.Persistent;
using RqCalc.Domain._Base;
using RqCalc.Domain.Equipment;

namespace RqCalc.Domain.Card;

public interface ICard : IDirectoryBase, ITypeObject<ICardType>, IBonusContainer<ICardBonus>, IVersionObject, ILegacyObject
{
    IReadOnlyCollection<IBuff> Buffs { get; }

    IReadOnlyCollection<ICardEquipmentSlot> EquipmentSlots { get; }

    IReadOnlyCollection<ICardEquipmentType> EquipmentTypes { get; }

    IReadOnlyCollection<ICardBuffDescription> BuffDescriptions { get; }

    CardGroupInfo? Group { get; }

    bool? IsSingleHandWeapon { get; }

    bool? IsMeleeWeapon { get; }

    IEquipmentClass? MinEquipmentClass { get; }

    ICardSet? Set { get; }
}