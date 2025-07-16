using RqCalc.Domain._Base;

namespace RqCalc.Domain.Equipment;

public interface IEquipmentElixir : IImageDirectoryBase, IBonusContainer<IEquipmentElixirBonus>, IVersionObject
{
    IReadOnlyCollection<IEquipmentSlot> Slots { get; }
}