using Framework.Persistent;
using RqCalc.Domain.Persistent._Base;
using RqCalc.Domain.Persistent.BonusType;

namespace RqCalc.Domain.Persistent.GuildTalent;

public interface IGuildTalentBonus : ITypeObject<IBonusType>, IPersistentDomainObjectBase
{
    IEnumerable<IGuildTalentBonusVariable> Variables { get; }

    IGuildTalent Talent { get; }
}