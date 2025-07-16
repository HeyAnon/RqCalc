using RqCalc.Domain._Base;

namespace RqCalc.Domain.Equipment;

public interface IEquipmentLevelForge : IPersistentDomainObjectBase
{
    int Level { get; }

    int EquipmentLevel { get; }

    int Hp { get; }
}