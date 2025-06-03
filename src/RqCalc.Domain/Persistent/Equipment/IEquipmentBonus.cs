using RqCalc.Domain.Persistent._Base;
using RqCalc.Domain.Persistent._Base._Blocks;

namespace RqCalc.Domain.Persistent.Equipment;

public interface IEquipmentBonus : IPersistentDomainObjectBase, IBonus
{
    IEquipment Equipment { get; }

    bool? Activate { get; }
}