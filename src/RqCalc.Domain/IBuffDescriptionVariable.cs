using RqCalc.Domain._Base;

namespace RqCalc.Domain;

public interface IBuffDescriptionVariable : ITextTemplateVariable, IPersistentDomainObjectBase
{
    IBuffDescription BuffDescription { get;}
}