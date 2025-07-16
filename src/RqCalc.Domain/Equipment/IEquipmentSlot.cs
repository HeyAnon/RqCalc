using RqCalc.Domain._Base;

namespace RqCalc.Domain.Equipment;

public interface IEquipmentSlot : IImageDirectoryBase
{
    IReadOnlyCollection<IEquipmentType> Types { get; }


    IState? State { get; }

    IEquipmentSlot? ExtraSlot { get; }

    IEquipmentSlot? PrimarySlot { get; }

    bool SharedStamp { get; }

    int Count { get; }

    bool? IsWeapon { get; }
}