using Framework.Persistent;
using RqCalc.Domain._Base;
using RqCalc.Domain.BonusType;

namespace RqCalc.Domain.GuildTalent;

public interface IGuildTalentBonus : ITypeObject<IBonusType>, IPersistentDomainObjectBase
{
    IEnumerable<IGuildTalentBonusVariable> Variables { get; }

    IGuildTalent Talent { get; }
}