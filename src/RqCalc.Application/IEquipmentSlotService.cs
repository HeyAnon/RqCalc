using RqCalc.Domain.Equipment;

namespace RqCalc.Application;

public interface IEquipmentSlotService
{
    IEquipmentSlot PrimaryWeaponSlot { get; }

    int GetMaxCardCount(IEquipmentSlot slot);
}