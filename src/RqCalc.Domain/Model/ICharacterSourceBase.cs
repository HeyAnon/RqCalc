using RqCalc.Domain.Persistent;
using RqCalc.Domain.Persistent._Base._Blocks;

namespace RqCalc.Domain.Model;

public interface ICharacterSourceBase : IClassObject, ILevelObject
{
    IGender Gender { get; }
}