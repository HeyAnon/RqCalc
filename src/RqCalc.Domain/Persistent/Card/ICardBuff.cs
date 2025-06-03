using RqCalc.Domain.Persistent._Base;
using RqCalc.Domain.Persistent._Base._Blocks;

namespace RqCalc.Domain.Persistent.Card;

public interface ICardBuffDescription : IPersistentDomainObjectBase, IBuffDescriptionElement
{
    ICard Card { get; }
}