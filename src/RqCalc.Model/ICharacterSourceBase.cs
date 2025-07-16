using RqCalc.Domain;

namespace RqCalc.Model;

public interface ICharacterSourceBase : IClassBuildSource
{
    IGender Gender { get; }
}