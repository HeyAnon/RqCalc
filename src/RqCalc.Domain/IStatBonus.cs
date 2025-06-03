using Framework.Persistent;
using RqCalc.Domain._Base;
using RqCalc.Domain.BonusType;
using RqCalc.Domain.Formula;

namespace RqCalc.Domain;

public interface IStatBonus : IPersistentDomainObjectBase, ITypeObject<IBonusType>
{
    IStat Stat { get; }

    IFormula Formula { get; }
}