using RqCalc.Domain.Persistent._Base;
using RqCalc.Domain.Persistent._Base._Blocks;

namespace RqCalc.Domain.Persistent.Equipment;

public interface IEquipmentCondition : IPersistentDomainObjectBase, IClassObject
{
    IEquipment Equipment { get; }
}