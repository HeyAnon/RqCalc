using RqCalc.Domain._Base;

namespace RqCalc.Domain.Equipment;

public interface IEquipmentClass : IDirectoryBase, ILevelObject
{
    int? MaxUpgradeLevel { get; }

    bool SharedStamp { get; }
}