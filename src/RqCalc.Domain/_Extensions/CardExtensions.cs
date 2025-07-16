using RqCalc.Domain.Card;
using RqCalc.Domain.Equipment;

namespace RqCalc.Domain._Extensions;

public static class CardExtensions
{
    public static bool IsAllowed(this ICard card, IEquipmentType equipmentType, IVersion version, IEquipmentClass equipmentClass)
    {
        if (card == null) throw new ArgumentNullException(nameof(card));
        if (equipmentType == null) throw new ArgumentNullException(nameof(equipmentType));
        if (version == null) throw new ArgumentNullException(nameof(version));
        if (equipmentClass == null) throw new ArgumentNullException(nameof(equipmentClass));

        return card.GetAllTypes(version).Contains(equipmentType) && card.IsAllowed(equipmentClass);
    }


    public static IEnumerable<IEquipmentSlot> GetSlots(this ICard card, IVersion version)
    {
        if (card == null) throw new ArgumentNullException(nameof(card));
        if (version == null) throw new ArgumentNullException(nameof(version));

        return card.EquipmentSlots.WhereVersion(version).Select(e => e.Slot);
    }

    public static IEnumerable<IEquipmentType> GetAllTypes(this ICard card, IVersion version)
    {
        if (card == null) throw new ArgumentNullException(nameof(card));
        if (version == null) throw new ArgumentNullException(nameof(version));

        return card.GetSlots(version).SelectMany(slot => slot.Types)
            .Concat(card.GetTypes(version)).Distinct().Where(card.IsAllowedWeapon);
    }

    public static IEnumerable<IEquipmentType> GetTypes(this ICard card, IVersion version)
    {
        if (card == null) throw new ArgumentNullException(nameof(card));
        if (version == null) throw new ArgumentNullException(nameof(version));

        return card.EquipmentTypes.WhereVersion(version).Select(e => e.Type);
    }

    private static bool IsAllowedWeapon(this ICard card, IEquipmentType equipmentType)
    {
        if (card == null) throw new ArgumentNullException(nameof(card));

        return (card.IsSingleHandWeapon == null || card.IsSingleHandWeapon == equipmentType.WeaponInfo.IsSingleHand)
               && (card.IsMeleeWeapon == null || card.IsMeleeWeapon == equipmentType.WeaponInfo.IsMelee);
    }

    private static bool IsAllowed(this ICard card, IEquipmentClass equipmentClass)
    {
        if (card == null) throw new ArgumentNullException(nameof(card));
        if (equipmentClass == null) throw new ArgumentNullException(nameof(equipmentClass));

        return card.Type.IsAllowed(equipmentClass)
               && (card.MinEquipmentClass == null || card.MinEquipmentClass.Id <= equipmentClass.Id);
    }

    private static bool IsAllowed(this ICardType cardType, IEquipmentClass equipmentClass)
    {
        if (cardType == null) throw new ArgumentNullException(nameof(cardType));
        if (equipmentClass == null) throw new ArgumentNullException(nameof(equipmentClass));

        return cardType.MaxEquipmentClass == null || cardType.MaxEquipmentClass.Id >= equipmentClass.Id;
    }
}