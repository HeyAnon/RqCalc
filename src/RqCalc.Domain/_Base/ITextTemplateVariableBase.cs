using Framework.Persistent;
using RqCalc.Core;

namespace RqCalc.Domain._Base;

public interface ITextTemplateVariableBase : IValueObject<decimal>, ITypeObject<TextTemplateVariableType>;