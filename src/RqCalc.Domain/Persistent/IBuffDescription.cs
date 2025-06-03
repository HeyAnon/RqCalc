using RqCalc.Domain.Persistent._Base;

namespace RqCalc.Domain.Persistent;

public interface IBuffDescription : IDirectoryBase
{
    IEnumerable<IBuffDescriptionVariable> Variables { get; }

    string Template { get; }

    bool IsStack { get; }
}