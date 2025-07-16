using RqCalc.Domain._Base;

namespace RqCalc.Domain.Equipment;

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