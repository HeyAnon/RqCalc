using System.ComponentModel.DataAnnotations.Schema;

using RqCalc.Domain.Talent;

namespace RqCalc.DataBase.EntityFramework.Talent;

[Table("TalentBuffDescription")]
public partial class TalentBuffDescription : BuffElement
{
    public virtual Talent Talent { get; set; } = null!;


    public bool IsPassive { get; set; }


    [Column("Talent_Id")]
    public int? TalentId { get; set; }
}

public partial class TalentBuffDescription : ITalentBuffDescription
{
    ITalent ITalentBuffDescription.Talent => this.Talent;
}