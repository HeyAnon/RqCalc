using System.ComponentModel.DataAnnotations.Schema;

using RqCalc.DataBase.EntityFramework._Base;
using RqCalc.Domain;
using RqCalc.Domain._Base;

namespace RqCalc.DataBase.EntityFramework;

[Table("AuraBonus")]
public partial class AuraBonus : Bonus
{
    public virtual Aura Aura { get; set; } = null!;

    public virtual Version? StartVersion { get; set; }

    public virtual Version? EndVersion { get; set; }


    public decimal? SharedValue { get; set; }


    [Column("Aura_Id")]
    public int AuraId { get; set; }

    [Column("StartVersion_Id")]
    public int? StartVersionId { get; set; }

    [Column("EndVersion_Id")]
    public int? EndVersionId { get; set; }
}

public partial class AuraBonus : IAuraBonus
{
    IVersion? IVersionObject.StartVersion => this.StartVersion;

    IVersion? IVersionObject.EndVersion => this.EndVersion;
}