using RqCalc.Domain.Persistent._Base;
using RqCalc.Domain.Persistent._Base._Blocks;
using RqCalc.Domain.Persistent.Equipment;

namespace RqCalc.Domain.Persistent.Card;

public interface ICardEquipmentType : IPersistentDomainObjectBase, IVersionObject
{
    ICard Card { get; }

    IEquipmentType Type { get; }
}