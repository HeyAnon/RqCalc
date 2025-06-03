using RqCalc.Domain.Persistent._Base;
using RqCalc.Domain.Persistent._Base._Blocks;
using RqCalc.Domain.Persistent.Formula;

namespace RqCalc.Domain.Persistent.BonusType;

public interface IBonusTypeVariable : IPersistentDomainObjectBase, IIndexObject
{
    IStat MultiplicityStat { get; }

    IBonusType BonusType { get; }

    bool HasSign { get; }
        
    IFormula MulFormula { get; }

    int? MultiplicityValue { get; }
}