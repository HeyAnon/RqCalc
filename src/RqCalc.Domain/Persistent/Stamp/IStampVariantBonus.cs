using Framework.Persistent;
using RqCalc.Domain.Persistent._Base;
using RqCalc.Domain.Persistent._Base._Blocks;

namespace RqCalc.Domain.Persistent.Stamp;

public interface IStampVariantBonus : IPersistentDomainObjectBase, IBonus, IValueObject<decimal>
{
    int QualityValue { get; }
}