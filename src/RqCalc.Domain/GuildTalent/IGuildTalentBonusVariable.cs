using RqCalc.Domain._Base;

namespace RqCalc.Domain.GuildTalent;

public interface IGuildTalentBonusVariable : IPersistentIdentityDomainObjectBase, IIndexObject
{
    int TalentVariableIndex { get; }

    IGuildTalentBonus GuildTalentBonus { get; }
}