using RqCalc.Domain.Persistent._Base;
using RqCalc.Domain.Persistent._Base._Blocks;

namespace RqCalc.Domain.Persistent.Stamp;

public interface IStamp : IDirectoryBase, ILegacyObject
{
    IEnumerable<IBuff> Buffs { get; }

    IEnumerable<IStampVariant> Variants { get; }

    IEnumerable<IStampEquipment> Equipments { get; }
}