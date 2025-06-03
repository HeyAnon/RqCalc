using RqCalc.Domain.Persistent._Base;

namespace RqCalc.Domain.Persistent.Equipment;

public interface IEquipmentElixirSlot : IPersistentDomainObjectBase
{
    IEquipmentElixir EquipmentElixir { get; }

    IEquipmentSlot EquipmentSlot { get; }
}