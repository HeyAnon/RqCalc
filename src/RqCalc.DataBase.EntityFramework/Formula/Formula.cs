using System.ComponentModel.DataAnnotations.Schema;

using RqCalc.DataBase.EntityFramework._Base;
using RqCalc.Domain.Formula;

namespace RqCalc.DataBase.EntityFramework.Formula;

[Table("Formula")]
public partial class Formula : PersistentDomainObjectBase
{
    public virtual HashSet<FormulaVariable> Variables { get; set; } = null!;
        

    public string Value { get; set; } = null!;

    public string Description { get; set; } = null!;

    public bool Enabled { get; set; }

    public override string ToString() => this.Value;
}

public partial class Formula : IFormula
{
    IReadOnlyCollection<IFormulaVariable> IFormula.Variables => this.Variables;
}