using Framework.Persistent;

using RqCalc.Domain._Base;

namespace RqCalc.Domain.CollectedStatistic;

public interface ICollectedItem : IImageDirectoryBase, IBonusContainer<ICollectedItemBonus>, IOrderObject<int>, IVersionObject
{
    ICollectedGroup Group { get; }

    IGender? Gender { get; }
}