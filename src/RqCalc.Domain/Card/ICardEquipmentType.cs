using RqCalc.Domain._Base;
using RqCalc.Domain.Equipment;

namespace RqCalc.Domain.Card;

public interface ICardEquipmentType : IPersistentDomainObjectBase, IVersionObject
{
    ICard Card { get; }

    IEquipmentType Type { get; }
}