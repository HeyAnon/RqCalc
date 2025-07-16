using Framework.Persistent;
using RqCalc.Domain._Base;

namespace RqCalc.Domain.Talent;

public interface ITalentBonus : IPersistentIdentityDomainObjectBase, IBonusBase, ISharedContainer, IValueObject<decimal>
{
    ITalent Talent { get; }

    IAura? AuraCondition { get; }

    IBuff? BuffCondition { get; }
}