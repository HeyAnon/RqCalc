using System.ComponentModel.DataAnnotations.Schema;

using RqCalc.Domain;
using RqCalc.Domain.Equipment;

namespace RqCalc.DataBase.EntityFramework.Equipment;

[Table("EquipmentCondition")]
public partial class EquipmentCondition
{
    public virtual Class Class { get; set; } = null!;

    public virtual Equipment Equipment { get; set; } = null!;

    [Column("Class_Id")]
    public int ClassId { get; set; }

    [Column("Equipment_Id")]
    public int EquipmentId { get; set; }
}

public partial class EquipmentCondition : IEquipmentCondition
{
    IEquipment IEquipmentCondition.Equipment => this.Equipment;

    IClass IEquipmentCondition.Class => this.Class;
}