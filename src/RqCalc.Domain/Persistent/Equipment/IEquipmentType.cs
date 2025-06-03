using RqCalc.Core;
using RqCalc.Domain.Persistent._Base;
using RqCalc.Domain.Persistent._Base._Blocks;

namespace RqCalc.Domain.Persistent.Equipment;

public interface IEquipmentType : IDirectoryBase, IBonusContainer<IEquipmentTypeBonus>
{
    IEnumerable<IEquipmentTypeCondition> Conditions { get; }
        
    IEnumerable<IEquipment> Equipments { get; }


    IEquipmentSlot Slot { get; }


    WeaponTypeInfo WeaponInfo { get; }
}