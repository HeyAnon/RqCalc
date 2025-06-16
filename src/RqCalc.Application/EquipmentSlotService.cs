using RqCalc.Application.Settings;
using RqCalc.Domain.Equipment;

namespace RqCalc.Application;

public class EquipmentSlotService(ApplicationSettings settings) : IEquipmentSlotService
{
    public int GetMaxCardCount(IEquipmentSlot slot)
    {
        return slot.IsWeapon switch
        {
            true => settings.WeaponCardCount,
            false => settings.EquipmentCardCount,
            _ => 0
        };
    }
}