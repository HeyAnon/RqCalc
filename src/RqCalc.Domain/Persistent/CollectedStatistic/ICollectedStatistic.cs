using Framework.Persistent;
using RqCalc.Domain.Persistent._Base;

namespace RqCalc.Domain.Persistent.CollectedStatistic;

public interface ICollectedStatistic : IDirectoryBase, IOrderObject<int>
{
}