using RqCalc.Domain.Persistent._Base;
using RqCalc.Domain.Persistent._Base._Blocks;

namespace RqCalc.Domain.Persistent.Equipment;

public interface IEquipmentClass : IDirectoryBase, ILevelObject
{
    int? MaxUpgradeLevel { get; }

    bool SharedStamp { get; }
}