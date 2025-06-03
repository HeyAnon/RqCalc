using RqCalc.Domain._Base;

namespace RqCalc.Domain.Stamp;

public interface IStamp : IDirectoryBase, ILegacyObject
{
    IEnumerable<IBuff> Buffs { get; }

    IEnumerable<IStampVariant> Variants { get; }

    IEnumerable<IStampEquipment> Equipments { get; }
}