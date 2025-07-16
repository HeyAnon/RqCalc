using RqCalc.Domain._Base;

namespace RqCalc.Domain.Equipment;

public interface IEquipmentElixirBonus : IPersistentDomainObjectBase, IBonus
{
    IEquipmentElixir EquipmentElixir { get; }
}