using System.ComponentModel.DataAnnotations.Schema;

using RqCalc.Domain;

namespace RqCalc.DataBase.EntityFramework;

[Table("ClassLevelHpBonus")]
public partial class ClassLevelHpBonus
{
    public virtual Class Class { get; set; } = null!;

    public int Value { get; set; }


    
    public int Level { get; set; }


    [Column("Class_Id")]
    public int ClassId { get; set; }
}

public partial class ClassLevelHpBonus : IClassLevelHpBonus
{
    IClass IClassLevelHpBonus.Class => this.Class;
}