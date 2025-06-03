using RqCalc.Domain.Persistent._Base;
using RqCalc.Domain.Persistent._Base._Blocks;

namespace RqCalc.Domain.Persistent.GuildTalent;

public interface IGuildTalentBonusVariable : IPersistentIdentityDomainObjectBase, IIndexObject
{
    int TalentVariableIndex { get; }

    IGuildTalentBonus GuildTalentBonus { get; }
}