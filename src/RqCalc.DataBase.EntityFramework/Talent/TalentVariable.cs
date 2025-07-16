using System.ComponentModel.DataAnnotations.Schema;

using RqCalc.Domain;
using RqCalc.Domain.Talent;

namespace RqCalc.DataBase.EntityFramework.Talent;

[Table("TalentVariable")]
public partial class TalentVariable
{
    public virtual Talent Talent { get; set; } = null!;


    public decimal Value { get; set; }

    public TextTemplateVariableType Type { get; set; }

    public int Index { get; set; }

    [Column("Talent_Id")]
    public int TalentId { get; set; }
}

public partial class TalentVariable : ITalentVariable
{
    ITalent ITalentVariable.Talent => this.Talent;
}