using RqCalc.Domain._Base;
using RqCalc.Domain.Formula;

namespace RqCalc.Domain.BonusType;

public interface IBonusTypeVariable : IPersistentDomainObjectBase, IIndexObject
{
    IStat? MultiplicityStat { get; }

    IBonusType BonusType { get; }

    bool HasSign { get; }
        
    IFormula? MulFormula { get; }

    int? MultiplicityValue { get; }
}