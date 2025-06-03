using RqCalc.Domain.Persistent._Base._Blocks;
using RqCalc.Domain.Persistent.Card;
using RqCalc.Domain.Persistent.Stamp;
using RqCalc.Domain.Persistent.Talent;

namespace RqCalc.Domain.Persistent;

public interface IBuff : IImageDirectoryBase, IBonusContainer<IBuffBonus>, IClassObject, ILevelObject, IStackObject, IVersionObject
{
    ITalent? TalentCondition { get; }

    bool AutoEnabled { get; }

    ICard Card { get; }

    IStamp Stamp { get; }

    bool IsNegate { get; }
}