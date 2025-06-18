using Framework.Persistent;

using RqCalc.Domain._Base;

namespace RqCalc.Domain.Formula;

public interface IFormulaVariable : IPersistentDomainObjectBase, ITypeObject<FormulaVariableType>, IIndexObject
{
    IFormula Formula { get; }

    IStat? TypeStat { get; }
}