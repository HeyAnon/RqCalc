using RqCalc.Domain._Base;

namespace RqCalc.Domain.Card;

public interface ICardBuffDescription : IPersistentDomainObjectBase, IBuffDescriptionElement
{
    ICard Card { get; }
}