using Framework.Persistent;

using RqCalc.Domain._Base;
using RqCalc.Domain.Equipment;

namespace RqCalc.Domain.Stamp;

public interface IStampEquipment : IPersistentDomainObjectBase, ITypeObject<IEquipmentType>, IVersionObject
{
    IStamp Stamp { get; }
}