using RqCalc.Domain._Base;

namespace RqCalc.Domain.Equipment;

public interface IEquipmentBonus : IPersistentDomainObjectBase, IBonus
{
    IEquipment Equipment { get; }

    bool? Activate { get; }
}