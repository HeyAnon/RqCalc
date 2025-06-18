using Framework.Persistent;

using RqCalc.Domain._Base;

namespace RqCalc.Domain.Formula;

public interface IFormula : IPersistentIdentityDomainObjectBase, IValueObject<string>, IDescriptionObject
{
    IReadOnlyCollection<IFormulaVariable> Variables { get; }

    bool Enabled { get; }
}