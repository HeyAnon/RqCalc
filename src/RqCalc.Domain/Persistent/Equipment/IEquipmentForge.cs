using RqCalc.Domain.Persistent._Base;

namespace RqCalc.Domain.Persistent.Equipment;

public interface IEquipmentForgeData
{
    decimal Attack { get; }

    decimal Defense { get; }

    int AllStatBonus { get; }
}

public interface IEquipmentForge : IPersistentDomainObjectBase, IEquipmentForgeData
{
    int Level { get; }
}