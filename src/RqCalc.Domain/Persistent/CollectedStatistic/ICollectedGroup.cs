using Framework.Persistent;
using RqCalc.Domain.Persistent._Base;

namespace RqCalc.Domain.Persistent.CollectedStatistic;

public interface ICollectedGroup : IDirectoryBase, IOrderObject<int>
{
    ICollectedStatistic Statistic { get; }

    IEnumerable<ICollectedItem> Items { get; }
}