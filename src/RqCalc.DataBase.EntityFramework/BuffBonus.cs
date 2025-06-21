using System.ComponentModel.DataAnnotations.Schema;

using RqCalc.DataBase.EntityFramework._Base;
using RqCalc.Domain;

namespace RqCalc.DataBase.EntityFramework;

[Table("BuffBonus")]
public partial class BuffBonus : Bonus
{
    public virtual Buff Buff { get; set; } = null!;


    [Column("Buff_Id")]
    public int BuffId { get; set; }
}

public partial class BuffBonus : IBuffBonus;