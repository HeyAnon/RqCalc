using RqCalc.Domain.Persistent._Base;
using RqCalc.Domain.Persistent._Base._Blocks;

namespace RqCalc.Domain.Persistent;

public interface IBuffDescriptionVariable : ITextTemplateVariable, IPersistentDomainObjectBase
{
    IBuffDescription BuffDescription { get;}
}