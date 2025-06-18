using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Framework.Persistent;

using RqCalc.Domain;
using RqCalc.Domain.Equipment;

namespace RqCalc.DataBase.EntityFramework.Equipment;

[Table("EquipmentTypeCondition")]
public partial class EquipmentTypeCondition
{
    public virtual EquipmentType Type { get; set; } = null!;

    public virtual Class Class { get; set; } = null!;


    [Key]
    [Column("Type_Id", Order = 0)]
    public int? TypeId { get; set; }

    [Key]
    [Column("Class_Id", Order = 1)]
    public int? ClassId { get; set; }
}

public partial class EquipmentTypeCondition : IEquipmentTypeCondition
{
    IEquipmentType ITypeObject<IEquipmentType>.Type => this.Type;

    IClass IEquipmentTypeCondition.Class => this.Class;
}