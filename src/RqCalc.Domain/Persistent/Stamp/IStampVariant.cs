using RqCalc.Domain.Persistent._Base;
using RqCalc.Domain.Persistent._Base._Blocks;

namespace RqCalc.Domain.Persistent.Stamp;

public interface IStampVariant : IPersistentIdentityDomainObjectBase, IBonusContainer<IStampVariantBonus>
{
    IStamp Stamp { get; }

    IStampColor Color { get; }
}