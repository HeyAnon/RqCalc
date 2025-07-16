using RqCalc.Domain._Base;

namespace RqCalc.Domain.Talent;

public interface ITalentVariable : ITextTemplateVariable, IPersistentDomainObjectBase
{
    ITalent Talent { get; }
}