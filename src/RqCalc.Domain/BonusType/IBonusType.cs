using RqCalc.Core;
using RqCalc.Domain._Base;

namespace RqCalc.Domain.BonusType;

public interface IBonusType : IPersistentIdentityDomainObjectBase
{
    IEnumerable<IBonusTypeVariable> Variables { get; }

    IEnumerable<IBonusTypeStat> Stats { get; }
        

    bool? IsMultiply { get; }

    bool IsSingle { get; }

    string Template { get; }

    StampQualityInfo StampQuality { get; }
}