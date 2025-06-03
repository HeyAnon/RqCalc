using RqCalc.Domain.Persistent._Base;

namespace RqCalc.Domain.Persistent;

public interface IClassSpecialization : IPersistentIdentityDomainObjectBase
{
    int? MaxLevel { get; }

    int BonusTalentCount { get; }
}