using Framework.Persistent;
using RqCalc.Domain._Base;

namespace RqCalc.Domain.Equipment;

public interface IEquipmentTypeCondition : IPersistentDomainObjectBase, ITypeObject<IEquipmentType>
{
    IClass Class { get; }
}