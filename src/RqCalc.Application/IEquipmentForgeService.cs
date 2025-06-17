using RqCalc.Domain.Equipment;

namespace RqCalc.Application;

public interface IEquipmentForgeService
{
    IEquipmentForge GetEquipmentForge(int level);

    int GetHpBonus(int equipmentLevel, int level);
}