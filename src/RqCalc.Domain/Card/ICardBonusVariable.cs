using RqCalc.Domain._Base;

namespace RqCalc.Domain.Card;

public interface ICardBonusVariable : IPersistentIdentityDomainObjectBase, IBonusVariable
{
    IEnumerable<ICardBonusVariableCondition> Conditions { get; }

    ICardBonus CardBonus { get; }
}