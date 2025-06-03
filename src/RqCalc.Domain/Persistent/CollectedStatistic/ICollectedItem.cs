using Framework.Persistent;
using RqCalc.Domain.Persistent._Base._Blocks;

namespace RqCalc.Domain.Persistent.CollectedStatistic;

public interface ICollectedItem : IImageDirectoryBase, IBonusContainer<ICollectedItemBonus>, IOrderObject<int>, IVersionObject
{
    ICollectedGroup Group { get; }

    IGender Gender { get; }
}