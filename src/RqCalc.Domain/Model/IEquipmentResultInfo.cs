using RqCalc.Core;
using RqCalc.Domain.Persistent._Base._Blocks;

namespace RqCalc.Domain.Model;

public interface IEquipmentResultInfo
{
    EquipmentUpgradeBaseInfo Upgrade { get; }

    IBonusContainer<IBonusBase> StampVariant { get; }

    IBonusContainer<IBonusBase> DynamicBonuses { get; }
}