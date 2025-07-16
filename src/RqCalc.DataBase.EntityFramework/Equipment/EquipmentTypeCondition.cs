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

    [Column("Type_Id")]
    public int TypeId { get; set; }

    [Column("Class_Id")]
    public int ClassId { get; set; }
}

public partial class EquipmentTypeCondition : IEquipmentTypeCondition
{
    IEquipmentType ITypeObject<IEquipmentType>.Type => this.Type;

    IClass IEquipmentTypeCondition.Class => this.Class;
}