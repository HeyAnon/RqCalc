using RqCalc.Domain.Persistent._Base;
using RqCalc.Domain.Persistent._Base._Blocks;

namespace RqCalc.Domain.Persistent.BonusType;

public interface IBonusTypeStat : IPersistentIdentityDomainObjectBase, IVariableObject
{
    IEnumerable<IBonusTypeStatCondition> Conditions { get; }


    IBonusType BonusType { get; }
        
    IStat Stat { get; }
}