using RqCalc.Domain._Base;

namespace RqCalc.Domain;

public interface IBuffDescription : IDirectoryBase
{
    IReadOnlyCollection<IBuffDescriptionVariable> Variables { get; }

    string Template { get; }

    bool IsStack { get; }
}