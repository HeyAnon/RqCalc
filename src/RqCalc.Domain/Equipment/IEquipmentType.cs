using RqCalc.Domain._Base;

namespace RqCalc.Domain.Equipment;

public interface IEquipmentType : IDirectoryBase, IBonusContainer<IEquipmentTypeBonus>
{
    IReadOnlyCollection<IEquipmentTypeCondition> Conditions { get; }
        
    IReadOnlyCollection<IEquipment> Equipments { get; }


    IEquipmentSlot Slot { get; }


    WeaponTypeInfo? WeaponInfo { get; }
}