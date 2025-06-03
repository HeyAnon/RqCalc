using RqCalc.Domain.Persistent._Base;
using RqCalc.Domain.Persistent._Base._Blocks;
using RqCalc.Domain.Persistent.Equipment;

namespace RqCalc.Domain.Persistent.Card;

public interface ICardEquipmentSlot : IPersistentDomainObjectBase, IVersionObject
{
    ICard Card { get; }

    IEquipmentSlot Slot { get; }
}