using RqCalc.Domain;
using RqCalc.Domain._Base;

namespace RqCalc.Model;

public interface IClassBuildSource : ILevelObject
{
    IClass Class { get; }
}