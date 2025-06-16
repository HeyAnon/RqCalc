using RqCalc.Domain.Equipment;

namespace RqCalc.Application;

public interface IEquipmentSlotService
{
    int GetMaxCardCount(IEquipmentSlot slot);
}