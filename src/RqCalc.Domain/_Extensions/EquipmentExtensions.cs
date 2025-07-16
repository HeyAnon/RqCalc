using Framework.Core;
using Framework.HierarchicalExpand;

using RqCalc.Domain.Equipment;

namespace RqCalc.Domain._Extensions;

public static class EquipmentExtensions
{
    public static bool IsActivate(this IEquipment equipment)
    {
        if (equipment == null) throw new ArgumentNullException(nameof(equipment));

        return equipment.Bonuses.Any(b => b.Activate != null);
    }

    public static bool IsAllowed(this IEquipment equipment, IState state)
    {
        if (equipment == null) throw new ArgumentNullException(nameof(equipment));
        if (state == null) throw new ArgumentNullException(nameof(state));

        return equipment.Type.Slot.State.Pipe(s => s == null || s == state);
    }

    public static IEnumerable<IClass> GetClassConditions(this IEquipment equipment)
    {
        if (equipment == null) throw new ArgumentNullException(nameof(equipment));

        var conditions = new[]
        {
            equipment.Type.Conditions.SelectMany(c => c.Class.GetAllChildren()).ToList(),
            equipment.Conditions.SelectMany(c => c.Class.GetAllChildren()).ToList()
        }.Where(cond => cond.Any()).ToList();

        return conditions.Any() ? conditions.Aggregate((l1, l2) => l1.Union(l2).ToList()).Where(@class => @class.Specialization.MaxLevel == null || @class.Specialization.MaxLevel >= equipment.Level) : [];
    }

    public static bool IsAllowed(this IEquipment equipment, IClass @class)
    {
        if (@class == null) throw new ArgumentNullException(nameof(@class));
        var conditions = equipment.GetClassConditions().ToList();

        return !conditions.Any() || conditions.Contains(@class);
    }

    public static bool IsDoubleHand(this IEquipment equipment)
    {
        if (equipment == null) throw new ArgumentNullException(nameof(equipment));

        return equipment.Type.WeaponInfo.Maybe(wi => !wi.IsSingleHand);
    }

    public static IEquipmentSlot? GetReverse(this IEquipmentSlot equipmentSlot)
    {
        if (equipmentSlot == null) throw new ArgumentNullException(nameof(equipmentSlot));

        return equipmentSlot.IsPrimarySlot() ? equipmentSlot.ExtraSlot
            : equipmentSlot.IsExtraSlot() ? equipmentSlot.PrimarySlot
            : null;
    }

    public static bool AllowWeapon(this IEquipmentSlot equipmentSlot, bool? doubleHand)
    {
        if (equipmentSlot == null) throw new ArgumentNullException(nameof(equipmentSlot));

        return equipmentSlot.Types.Any(t => doubleHand == true  ? t.WeaponInfo.Maybe(wi => !wi.IsSingleHand)
            : doubleHand == false ? t.WeaponInfo.Maybe(wi =>  wi.IsSingleHand)
            : t.WeaponInfo != null);
    }
}