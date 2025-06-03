using Framework.Persistent;
using RqCalc.Domain.Persistent._Base;
using RqCalc.Domain.Persistent._Base._Blocks;

namespace RqCalc.Domain.Persistent.Talent;

public interface ITalentBonus : IPersistentIdentityDomainObjectBase, IBonusBase, ISharedContainer, IValueObject<decimal>
{
    ITalent Talent { get; }

    IAura? AuraCondition { get; }

    IBuff? BuffCondition { get; }
}