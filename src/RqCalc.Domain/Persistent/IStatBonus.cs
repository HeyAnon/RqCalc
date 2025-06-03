using Framework.Persistent;
using RqCalc.Domain.Persistent._Base;
using RqCalc.Domain.Persistent.BonusType;
using RqCalc.Domain.Persistent.Formula;

namespace RqCalc.Domain.Persistent;

public interface IStatBonus : IPersistentDomainObjectBase, ITypeObject<IBonusType>
{
    IStat Stat { get; }

    IFormula Formula { get; }
}