using RqCalc.Domain.Persistent._Base;
using RqCalc.Domain.Persistent._Base._Blocks;

namespace RqCalc.Domain.Persistent.Card;

public interface ICardBonusVariable : IPersistentIdentityDomainObjectBase, IBonusVariable
{
    IEnumerable<ICardBonusVariableCondition> Conditions { get; }

    ICardBonus CardBonus { get; }
}