using RqCalc.Domain.Persistent._Base;

namespace RqCalc.Domain.Persistent;

public interface IRace : IDirectoryBase
{
    bool IsPvP { get; }
}