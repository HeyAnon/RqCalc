using System.ComponentModel.DataAnnotations.Schema;

using RqCalc.DataBase.EntityFramework._Base;
using RqCalc.Domain.Equipment;

namespace RqCalc.DataBase.EntityFramework.Equipment;

[Table("EquipmentTypeBonus")]
public partial class EquipmentTypeBonus : BonusBase
{
    public virtual EquipmentType EquipmentType { get; set; } = null!;

    [Column("EquipmentType_Id")]
    public int? EquipmentTypeId { get; set; }
}

public partial class EquipmentTypeBonus : IEquipmentTypeBonus
{
        
}