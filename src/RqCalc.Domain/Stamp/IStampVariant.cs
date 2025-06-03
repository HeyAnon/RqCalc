using RqCalc.Domain._Base;

namespace RqCalc.Domain.Stamp;

public interface IStampVariant : IPersistentIdentityDomainObjectBase, IBonusContainer<IStampVariantBonus>
{
    IStamp Stamp { get; }

    IStampColor Color { get; }
}