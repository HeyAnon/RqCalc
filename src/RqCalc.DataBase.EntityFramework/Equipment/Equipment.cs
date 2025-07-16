using System.ComponentModel.DataAnnotations.Schema;

using Framework.Core;

using RqCalc.DataBase.EntityFramework._Base;
using RqCalc.Domain;
using RqCalc.Domain._Base;
using RqCalc.Domain.Card;
using RqCalc.Domain.Equipment;

namespace RqCalc.DataBase.EntityFramework.Equipment;

[Table("Equipment")]
public partial class Equipment : ImageDirectoryBase
{
    public virtual HashSet<EquipmentCondition> Conditions { get; set; } = null!;

    public virtual HashSet<EquipmentBonus> Bonuses { get; set; } = null!;

    public virtual EquipmentType Type { get; set; } = null!;

    public virtual Card.Card? PrimaryCard { get; set; }


    public virtual Version? StartVersion { get; set; }

    public virtual Version? EndVersion { get; set; }


    public virtual Gender? Gender { get; set; }

    public virtual Image? CostumeImage { get; set; }

    public bool IsPersonal { get; set; }



    public int? InternalLevel { get; set; }

    public int Level { get; set; }

    public int? Attack { get; set; }

    public decimal? AttackSpeed { get; set; }

    public int? Defense { get; set; }

    [Column("CostumeImage_Id")]
    public int? CostumeImageId { get; set; }

    [Column("Type_Id")]
    public int TypeId { get; set; }

    [Column("Gender_Id")]
    public int? GenderId { get; set; }

    [Column("PrimaryCard_Id")]
    public int? PrimaryCardId { get; set; }


    [Column("StartVersion_Id")]
    public int? StartVersionId { get; set; }

    [Column("EndVersion_Id")]
    public int? EndVersionId { get; set; }
}

public partial class Equipment : IEquipment
{
    private readonly Lazy<EquipmentBaseInfo?> lazyInfo;


    public Equipment() => this.lazyInfo = LazyHelper.Create(this.GetInfo);

    public EquipmentBaseInfo? Info => this.lazyInfo.Value;


    IReadOnlyCollection<IEquipmentCondition> IEquipment.Conditions => this.Conditions;

    IReadOnlyCollection<IEquipmentBonus> IBonusContainer<IEquipmentBonus>.Bonuses => this.Bonuses;

    IEquipmentType Framework.Persistent.ITypeObject<IEquipmentType>.Type => this.Type;

    IGender? IEquipment.Gender => this.Gender;

    ICard? IEquipment.PrimaryCard => this.PrimaryCard;

    IImage? IEquipment.CostumeImage => this.CostumeImage;

    IVersion? IVersionObject.StartVersion => this.StartVersion;

    IVersion? IVersionObject.EndVersion => this.EndVersion;


    private EquipmentBaseInfo? GetInfo()
    {
        if (this.InternalLevel == null && this.Attack == null && this.AttackSpeed == null && this.Defense == null)
        {
            return null;
        }
        else if (this.InternalLevel != null && this.Attack != null && this.AttackSpeed != null)
        {
            if (this.Defense == null)
            {
                return new WeaponInfo(this.InternalLevel.Value, this.Attack.Value, this.AttackSpeed.Value);
            }
            else
            {
                return new DefenseWeaponInfo(this.InternalLevel.Value, this.Attack.Value, this.AttackSpeed.Value, this.Defense.Value);
            }
        }
        else if (this.InternalLevel != null && this.Attack == null && this.AttackSpeed == null && this.Defense != null)
        {
            return new EquipmentInfo(this.InternalLevel.Value, this.Defense.Value);
        }
        else
        {
            throw new Exception("Invalid EquipmentInfo");
        }
    }


    bool IEquipment.IsCostume => this.CostumeImageId.HasValue;
}