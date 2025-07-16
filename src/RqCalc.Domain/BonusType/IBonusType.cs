using RqCalc.Domain._Base;

namespace RqCalc.Domain.BonusType;

public interface IBonusType : IPersistentIdentityDomainObjectBase
{
    IReadOnlyCollection<IBonusTypeVariable> Variables { get; }

    IReadOnlyCollection<IBonusTypeStat> Stats { get; }
        

    bool? IsMultiply { get; }

    bool IsSingle { get; }

    string Template { get; }

    StampQualityInfo? StampQuality { get; }
}