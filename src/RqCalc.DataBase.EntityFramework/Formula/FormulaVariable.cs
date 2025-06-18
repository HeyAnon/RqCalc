using System.ComponentModel.DataAnnotations.Schema;

using RqCalc.DataBase.EntityFramework._Base;
using RqCalc.Domain;
using RqCalc.Domain.Formula;

namespace RqCalc.DataBase.EntityFramework.Formula;

[Table("FormulaVariable")]
public partial class FormulaVariable : PersistentDomainObjectBase
{
    public virtual Formula Formula { get; set; } = null!;

    public virtual Stat? TypeStat { get; set; }
        


    public int Index { get; set; }
        
    public FormulaVariableType Type { get; set; }


    [Column("Formula_Id")]
    public int? FormulaId { get; set; }


    [Column("TypeStat_Id")]
    public int? TypeStatId { get; set; }
}

public partial class FormulaVariable : IFormulaVariable
{
    IFormula IFormulaVariable.Formula => this.Formula;

    IStat? IFormulaVariable.TypeStat => this.TypeStat;
}