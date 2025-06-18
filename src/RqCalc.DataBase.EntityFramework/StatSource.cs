using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RqCalc.DataBase.EntityFramework;

[Table("StatSource")]
public class StatSource
{
    public virtual Stat Stat { get; set; } = null!;

    public virtual Formula.Formula Formula { get; set; } = null!;


    [Key]
    [Column("Stat_Id", Order = 0)]
    public int? StatId { get; set; }

    [Key]
    [Column("Formula_Id", Order = 1)]
    public int? FormulaId { get; set; }
}