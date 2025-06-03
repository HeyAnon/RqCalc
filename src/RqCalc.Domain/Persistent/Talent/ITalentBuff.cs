using RqCalc.Domain.Persistent._Base;
using RqCalc.Domain.Persistent._Base._Blocks;

namespace RqCalc.Domain.Persistent.Talent;

public interface ITalentBuffDescription : IPersistentDomainObjectBase, IBuffDescriptionElement
{
    ITalent Talent { get; }
        
    bool IsPassive { get; }
}