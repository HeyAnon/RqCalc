using Framework.Core;
using RqCalc.Domain.Equipment;

namespace RqCalc.Domain._Extensions;

public static class EquipmentSlotExtensions
{
    public static bool HasReverseSlot(this IEquipmentSlot equipmentSlot)
    {
        if (equipmentSlot == null) throw new ArgumentNullException(nameof(equipmentSlot));

        return equipmentSlot.IsPrimarySlot () || equipmentSlot.IsExtraSlot();
    }

    public static bool IsPrimarySlot(this IEquipmentSlot equipmentSlot)
    {
        if (equipmentSlot == null) throw new ArgumentNullException(nameof(equipmentSlot));

        return equipmentSlot.ExtraSlot != null;
    }

    public static bool IsExtraSlot(this IEquipmentSlot equipmentSlot)
    {
        if (equipmentSlot == null) throw new ArgumentNullException(nameof(equipmentSlot));

        return equipmentSlot.PrimarySlot != null;
    }

    public static bool AllowSingleHand(this IEquipmentSlot equipmentSlot, IClass @class)
    {
        if (equipmentSlot == null) throw new ArgumentNullException(nameof(equipmentSlot));

        return equipmentSlot.Types.Any(t => t.WeaponInfo.Maybe(wi => wi.IsSingleHand) && @class.IsSubsetOf(t.Conditions.Select(c => c.Class)));
    }
}