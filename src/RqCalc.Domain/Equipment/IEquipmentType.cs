using RqCalc.Core;
using RqCalc.Domain._Base;

namespace RqCalc.Domain.Equipment;

public interface IEquipmentType : IDirectoryBase, IBonusContainer<IEquipmentTypeBonus>
{
    IEnumerable<IEquipmentTypeCondition> Conditions { get; }
        
    IEnumerable<IEquipment> Equipments { get; }


    IEquipmentSlot Slot { get; }


    WeaponTypeInfo? WeaponInfo { get; }
}