using RqCalc.Domain._Base;

namespace RqCalc.Domain.BonusType;

public interface IBonusTypeStat : IPersistentIdentityDomainObjectBase, IVariableObject
{
    IEnumerable<IBonusTypeStatCondition> Conditions { get; }


    IBonusType BonusType { get; }
        
    IStat Stat { get; }
}