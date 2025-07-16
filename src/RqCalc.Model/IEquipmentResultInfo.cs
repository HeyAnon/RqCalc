using RqCalc.Domain;
using RqCalc.Domain._Base;

namespace RqCalc.Model;

public interface IEquipmentResultInfo
{
    EquipmentUpgradeBaseInfo? Upgrade { get; }

    IBonusContainer<IBonusBase>? StampVariant { get; }

    IBonusContainer<IBonusBase>? DynamicBonuses { get; }
}