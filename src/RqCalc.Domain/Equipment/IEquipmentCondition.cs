using RqCalc.Domain._Base;

namespace RqCalc.Domain.Equipment;

public interface IEquipmentCondition : IPersistentDomainObjectBase
{
    IClass Class { get; }

    IEquipment Equipment { get; }
}