using Framework.Persistent;

using RqCalc.Domain._Base;

namespace RqCalc.Domain.CollectedStatistic;

public interface ICollectedGroup : IDirectoryBase, IOrderObject<int>
{
    ICollectedStatistic Statistic { get; }

    IReadOnlyCollection<ICollectedItem> Items { get; }
}