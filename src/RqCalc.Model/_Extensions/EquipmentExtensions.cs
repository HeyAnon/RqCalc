using Framework.Core;

using RqCalc.Domain._Extensions;
using RqCalc.Domain.Equipment;

namespace RqCalc.Model._Extensions;

public static class EquipmentExtensions
{
    public static bool IsAllowed(this IEquipment equipment, ICharacterSourceBase character, IEquipmentSlot slot)
    {
        return equipment.IsAllowed(character) && (equipment.Type.Slot == slot || (character.Class.AllowExtraWeapon && slot.IsExtraSlot() && equipment.Type.Slot == slot.PrimarySlot));
    }

    public static CharacterEquipmentIdentity? GetReverse(this CharacterEquipmentIdentity equipmentIdentity)
    {
        return equipmentIdentity.Slot.GetReverse().Maybe(slot => equipmentIdentity with { Slot = slot });
    }

    public static bool IsAllowed(this IEquipment equipment, ICharacterSourceBase character)
    {
        return equipment.Level <= character.Level
               && (equipment.Gender == null || equipment.Gender == character.Gender)
               && equipment.IsAllowed(character.Class);
    }
}
