using RqCalc.Domain.Persistent._Base;

namespace RqCalc.Domain.Persistent.Equipment;

public interface IEquipmentLevelForge : IPersistentDomainObjectBase
{
    int Level { get; }

    int EquipmentLevel { get; }

    int Hp { get; }
}