using RqCalc.Domain._Base;

namespace RqCalc.Domain;

public interface IRace : IDirectoryBase
{
    bool IsPvP { get; }
}