using Framework.Persistent;
using RqCalc.Domain.Persistent.BonusType;

namespace RqCalc.Domain.Persistent._Base._Blocks;

public interface IBonusBase : ITypeObject<IBonusType>
{
    IReadOnlyList<decimal> Variables { get; }
}