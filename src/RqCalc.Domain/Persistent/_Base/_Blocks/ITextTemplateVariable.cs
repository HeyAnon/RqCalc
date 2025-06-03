using Framework.Persistent;
using RqCalc.Core;

namespace RqCalc.Domain.Persistent._Base._Blocks;

public interface ITextTemplateVariable : ITextTemplateVariableBase, IIndexObject
{
}

public interface ITextTemplateVariableBase : IValueObject<decimal>, ITypeObject<TextTemplateVariableType>
{
}