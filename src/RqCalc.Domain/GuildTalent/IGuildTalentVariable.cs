using Framework.Persistent;

using RqCalc.Domain._Base;

namespace RqCalc.Domain.GuildTalent;

public interface IGuildTalentVariable : IIndexObject, IValueObject<decimal>, IPersistentDomainObjectBase
{
    IGuildTalent Talent { get; }

    int Points { get; }
}