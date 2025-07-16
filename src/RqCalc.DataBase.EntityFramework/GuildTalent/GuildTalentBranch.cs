using System.ComponentModel.DataAnnotations.Schema;

using RqCalc.DataBase.EntityFramework._Base;
using RqCalc.Domain.GuildTalent;

namespace RqCalc.DataBase.EntityFramework.GuildTalent;

[Table("GuildTalentBranch")]
public partial class GuildTalentBranch : DirectoryBase
{
    public virtual HashSet<GuildTalent> Talents { get; set; } = null!;

    public int MaxPoints { get; set; }
}

public partial class GuildTalentBranch : IGuildTalentBranch
{
    IReadOnlyCollection<IGuildTalent> IGuildTalentBranch.Talents => this.Talents;
}