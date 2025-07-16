using System.ComponentModel.DataAnnotations.Schema;

using RqCalc.DataBase.EntityFramework._Base;
using RqCalc.Domain;
using RqCalc.Domain.BonusType;
using RqCalc.Domain.Formula;

namespace RqCalc.DataBase.EntityFramework.BonusType;

[Table("BonusTypeVariable")]
public partial class BonusTypeVariable : PersistentDomainObjectBase
{
    public virtual Stat? MultiplicityStat { get; set; }
        
    public virtual BonusType BonusType { get; set; } = null!;

    public virtual Formula.Formula? MulFormula { get; set; }


    public int? MultiplicityValue { get; set; }

    public bool HasSign { get; set; }

    public int Index { get; set; }

        


    [Column("BonusType_Id")]
    public int? BonusTypeId { get; set; }

    [Column("MultiplicityStat_Id")]
    public int? MultiplicityStatId { get; set; }

    [Column("MulFormula_Id")]
    public int? MulFormulaId { get; set; }
}

public partial class BonusTypeVariable : IBonusTypeVariable
{
    IStat? IBonusTypeVariable.MultiplicityStat => this.MultiplicityStat;

    IBonusType IBonusTypeVariable.BonusType => this.BonusType;

    IFormula? IBonusTypeVariable.MulFormula => this.MulFormula;
}