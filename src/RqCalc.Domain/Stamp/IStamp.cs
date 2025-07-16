using RqCalc.Domain._Base;

namespace RqCalc.Domain.Stamp;

public interface IStamp : IDirectoryBase, ILegacyObject
{
    IReadOnlyCollection<IBuff> Buffs { get; }

    IReadOnlyCollection<IStampVariant> Variants { get; }

    IReadOnlyCollection<IStampEquipment> Equipments { get; }
}