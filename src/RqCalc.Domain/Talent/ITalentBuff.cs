using RqCalc.Domain._Base;

namespace RqCalc.Domain.Talent;

public interface ITalentBuffDescription : IPersistentDomainObjectBase, IBuffDescriptionElement
{
    ITalent Talent { get; }
        
    bool IsPassive { get; }
}