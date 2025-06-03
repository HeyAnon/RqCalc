using Framework.Persistent;
using RqCalc.Domain.BonusType;

namespace RqCalc.Domain._Base;

public interface IBonusBase : ITypeObject<IBonusType>
{
    IReadOnlyList<decimal> Variables { get; }
}