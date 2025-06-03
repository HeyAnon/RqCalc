using RqCalc.Domain.Persistent._Base._Blocks;

namespace RqCalc.Domain.Persistent.Equipment;

public interface IEquipmentElixir : IImageDirectoryBase, IBonusContainer<IEquipmentElixirBonus>, IVersionObject
{
    IEnumerable<IEquipmentSlot> Slots { get; }
}