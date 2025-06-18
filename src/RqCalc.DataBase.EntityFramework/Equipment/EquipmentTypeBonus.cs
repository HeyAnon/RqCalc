using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using RqCalc.DataBase.EntityFramework._Base;
using RqCalc.Domain.Equipment;

namespace RqCalc.DataBase.EntityFramework.Equipment;

[Table("EquipmentTypeBonus")]
public partial class EquipmentTypeBonus : BonusBase
{
    public virtual EquipmentType EquipmentType { get; set; } = null!;


    [Key]
    [Column("Type_Id", Order = 1)]
    public override int TypeId
    {
        get { return base.TypeId; }
        set { base.TypeId = value; }
    }

    [Key]
    [Column("EquipmentType_Id", Order = 0)]
    public int? EquipmentTypeId { get; set; }
}

public partial class EquipmentTypeBonus : IEquipmentTypeBonus
{
        
}