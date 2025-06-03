using RqCalc.Core;
using RqCalc.Domain.Persistent._Base._Blocks;

namespace RqCalc.Domain.Model;

public class EquipmentResultInfo : IEquipmentResultInfo
{
    public EquipmentUpgradeBaseInfo Upgrade { get; set; }
        
    public IBonusContainer<IBonusBase> StampVariant { get; set; }

    public IBonusContainer<IBonusBase> DynamicBonuses { get; set; }
}