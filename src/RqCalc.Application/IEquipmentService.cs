using RqCalc.Domain.Equipment;

namespace RqCalc.Application;

public interface IEquipmentService
{
    IEquipmentClass GetEquipmentClass(IEquipment equipment);

    int GetMaxUpgradeLevel(IEquipment equipment);
}