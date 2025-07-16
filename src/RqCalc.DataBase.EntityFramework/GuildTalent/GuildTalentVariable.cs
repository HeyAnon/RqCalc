using System.ComponentModel.DataAnnotations.Schema;

using RqCalc.Domain.GuildTalent;

namespace RqCalc.DataBase.EntityFramework.GuildTalent;

[Table("GuildTalentVariable")]
public partial class GuildTalentVariable
{
    public virtual GuildTalent Talent { get; set; } = null!;


    public decimal Value { get; set; }

    public int Index { get; set; }

    public int Points { get; set; }

    [Column("Talent_Id")]
    public int TalentId { get; set; }
}

public partial class GuildTalentVariable : IGuildTalentVariable
{
    IGuildTalent IGuildTalentVariable.Talent => this.Talent;
}