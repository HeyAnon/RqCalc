using System.ComponentModel.DataAnnotations.Schema;

using Framework.Core;

using RqCalc.DataBase.EntityFramework._Base;
using RqCalc.Domain;
using RqCalc.Domain._Base;
using RqCalc.Domain.Equipment;

namespace RqCalc.DataBase.EntityFramework.Equipment;

[Table("EquipmentType")]
public partial class EquipmentType : DirectoryBase
{
    public virtual HashSet<EquipmentTypeBonus> Bonuses { get; set; } = null!;
        
    public virtual HashSet<EquipmentTypeCondition> Conditions { get; set; } = null!;

    public virtual HashSet<Equipment> Equipments { get; set; } = null!;


    public virtual EquipmentSlot Slot { get; set; } = null!;


    public bool? IsMeleeWeapon { get; set; }

    public bool? IsSingleHandWeapon { get; set; }


    [Column("Slot_Id")]
    public int? SlotId { get; set; }
}

public partial class EquipmentType : IEquipmentType
{
    private readonly Lazy<WeaponTypeInfo?> lazyWeaponInfo;


    public EquipmentType() => this.lazyWeaponInfo = LazyHelper.Create(this.GetWeaponInfo);

    public WeaponTypeInfo? WeaponInfo => this.lazyWeaponInfo.Value;


    IReadOnlyCollection<IEquipmentTypeBonus> IBonusContainer<IEquipmentTypeBonus>.Bonuses => this.Bonuses;

    IReadOnlyCollection<IEquipmentTypeCondition> IEquipmentType.Conditions => this.Conditions;

    IReadOnlyCollection<IEquipment> IEquipmentType.Equipments => this.Equipments;

    IEquipmentSlot IEquipmentType.Slot => this.Slot;


    private WeaponTypeInfo? GetWeaponInfo()
    {
        if (this.IsMeleeWeapon != null && this.IsSingleHandWeapon != null)
        {
            return new WeaponTypeInfo(this.IsMeleeWeapon.Value, this.IsSingleHandWeapon.Value);
        }
        else if (this.IsMeleeWeapon == null && this.IsSingleHandWeapon == null)
        {
            return null;
        }
        else
        {
            throw new Exception("Invalid WeaponInfo");
        }
    }
}