using RqCalc.Domain.Persistent._Base;
using RqCalc.Domain.Persistent._Base._Blocks;

namespace RqCalc.Domain.Persistent.Equipment;

public interface IEquipmentElixirBonus : IPersistentDomainObjectBase, IBonus
{
    IEquipmentElixir EquipmentElixir { get; }
}