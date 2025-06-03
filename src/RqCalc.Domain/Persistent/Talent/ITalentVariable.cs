using RqCalc.Domain.Persistent._Base;
using RqCalc.Domain.Persistent._Base._Blocks;

namespace RqCalc.Domain.Persistent.Talent;

public interface ITalentVariable : ITextTemplateVariable, IPersistentDomainObjectBase
{
    ITalent Talent { get; }
}