using Framework.Persistent;
using RqCalc.Domain._Base;

namespace RqCalc.Domain.Stamp;

public interface IStampVariantBonus : IPersistentDomainObjectBase, IBonus, IValueObject<decimal>
{
    int QualityValue { get; }
}