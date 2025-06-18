using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using RqCalc.Domain;
using RqCalc.Domain.BonusType;
using RqCalc.Domain.Formula;

namespace RqCalc.DataBase.EntityFramework;

[Table("StatBonus")]
public partial class StatBonus
{
    public virtual Stat Stat { get; set; } = null!;
        
    public virtual BonusType.BonusType Type { get; set; } = null!;

    public virtual Formula.Formula Formula { get; set; } = null!;


    [Column("Formula_Id")]
    public int? FormulaId { get; set; }
        
    [Key]
    [Column("Stat_Id", Order = 0)]
    public int? StatId { get; set; }

    [Key]
    [Column("Type_Id", Order = 1)]
    public virtual int? TypeId { get; set; }
}


public partial class StatBonus : IStatBonus
{
    IStat IStatBonus.Stat => this.Stat;

    IFormula IStatBonus.Formula => this.Formula;

    IBonusType Framework.Persistent.ITypeObject<IBonusType>.Type => this.Type;
}