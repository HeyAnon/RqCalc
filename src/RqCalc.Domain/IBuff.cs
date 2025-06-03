using RqCalc.Domain._Base;
using RqCalc.Domain.Card;
using RqCalc.Domain.Stamp;
using RqCalc.Domain.Talent;

namespace RqCalc.Domain;

public interface IBuff : IImageDirectoryBase, IBonusContainer<IBuffBonus>, ILevelObject, IStackObject, IVersionObject
{
    IClass? Class { get; }

    ITalent? TalentCondition { get; }

    bool AutoEnabled { get; }

    ICard? Card { get; }

    IStamp? Stamp { get; }

    bool IsNegate { get; }
}