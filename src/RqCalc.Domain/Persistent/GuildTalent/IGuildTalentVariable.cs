using Framework.Persistent;
using RqCalc.Domain.Persistent._Base;
using RqCalc.Domain.Persistent._Base._Blocks;

namespace RqCalc.Domain.Persistent.GuildTalent;

public interface IGuildTalentVariable : IIndexObject, IValueObject<decimal>, IPersistentDomainObjectBase
{
    IGuildTalent Talent { get; }

    int Points { get; }
}