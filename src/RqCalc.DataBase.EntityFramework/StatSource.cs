using System.ComponentModel.DataAnnotations.Schema;

namespace RqCalc.DataBase.EntityFramework;

[Table("StatSource")]
public class StatSource
{
    public virtual Stat Stat { get; set; } = null!;

    public virtual Formula.Formula Formula { get; set; } = null!;


    [Column("Stat_Id")]
    public int StatId { get; set; }

    [Column("Formula_Id")]
    public int FormulaId { get; set; }
}