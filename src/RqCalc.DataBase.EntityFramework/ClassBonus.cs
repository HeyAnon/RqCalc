using System.ComponentModel.DataAnnotations.Schema;

using RqCalc.DataBase.EntityFramework._Base;
using RqCalc.Domain;

namespace RqCalc.DataBase.EntityFramework;

[Table("ClassBonus")]
public partial class ClassBonus : BonusBase
{
    public virtual Class Class { get; set; } = null!;


    [Column("Type_Id")]
    public override int TypeId
    {
        get => base.TypeId;
        set => base.TypeId = value;
    }

    [Column("Class_Id")]
    public int ClassId { get; set; }
}

public partial class ClassBonus : IClassBonus
{
        
}