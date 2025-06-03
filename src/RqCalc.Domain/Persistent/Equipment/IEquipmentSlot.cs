using RqCalc.Domain.Persistent._Base._Blocks;

namespace RqCalc.Domain.Persistent.Equipment;

public interface IEquipmentSlot : IImageDirectoryBase
{
    IEnumerable<IEquipmentType> Types { get; }


    IState? State { get; }

    IEquipmentSlot ExtraSlot { get; }

    IEquipmentSlot PrimarySlot { get; }

    bool SharedStamp { get; }

    int Count { get; }

    bool? IsWeapon { get; }
}