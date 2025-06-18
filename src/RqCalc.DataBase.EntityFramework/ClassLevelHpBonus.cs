using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using RqCalc.Domain;

namespace RqCalc.DataBase.EntityFramework;

[Table("ClassLevelHpBonus")]
public partial class ClassLevelHpBonus
{
    public virtual Class Class { get; set; } = null!;

    public int Value { get; set; }


    [Key]
    [Column(Order = 1)]
    public int Level { get; set; }


    [Key]
    [Column("Class_Id", Order = 0)]
    public int? ClassId { get; set; }
}

public partial class ClassLevelHpBonus : IClassLevelHpBonus
{
    IClass IClassLevelHpBonus.Class => this.Class;
}