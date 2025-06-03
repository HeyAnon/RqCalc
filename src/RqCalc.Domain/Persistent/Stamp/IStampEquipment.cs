using Framework.Persistent;
using RqCalc.Domain.Persistent._Base;
using RqCalc.Domain.Persistent._Base._Blocks;
using RqCalc.Domain.Persistent.Equipment;

namespace RqCalc.Domain.Persistent.Stamp;

public interface IStampEquipment : IPersistentDomainObjectBase, ITypeObject<IEquipmentType>, IVersionObject
{
    IStamp Stamp { get; }
}