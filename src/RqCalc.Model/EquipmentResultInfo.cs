using RqCalc.Domain;
using RqCalc.Domain._Base;

namespace RqCalc.Model;

public class EquipmentResultInfo : IEquipmentResultInfo
{
    public EquipmentUpgradeBaseInfo? Upgrade { get; set; }
        
    public IBonusContainer<IBonusBase>? StampVariant { get; set; }

    public IBonusContainer<IBonusBase>? DynamicBonuses { get; set; }
}