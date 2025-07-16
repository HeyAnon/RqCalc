using RqCalc.Domain._Base;

namespace RqCalc.Domain.Equipment;

public interface IEquipmentElixirSlot : IPersistentDomainObjectBase
{
    IEquipmentElixir EquipmentElixir { get; }

    IEquipmentSlot EquipmentSlot { get; }
}