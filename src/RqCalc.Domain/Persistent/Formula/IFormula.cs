using Framework.Persistent;
using RqCalc.Domain.Persistent._Base;

namespace RqCalc.Domain.Persistent.Formula;

public interface IFormula : IPersistentIdentityDomainObjectBase, IValueObject<string>, IDescriptionObject
{
    IEnumerable<IFormulaVariable> Variables { get; }

    bool Enabled { get; }
}