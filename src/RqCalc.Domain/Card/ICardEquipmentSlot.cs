using RqCalc.Domain._Base;
using RqCalc.Domain.Equipment;

namespace RqCalc.Domain.Card;

public interface ICardEquipmentSlot : IPersistentDomainObjectBase, IVersionObject
{
    ICard Card { get; }

    IEquipmentSlot Slot { get; }
}