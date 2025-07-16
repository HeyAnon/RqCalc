using RqCalc.Domain._Base;

namespace RqCalc.Domain;

public interface IClassSpecialization : IPersistentIdentityDomainObjectBase
{
    int? MaxLevel { get; }

    int BonusTalentCount { get; }
}