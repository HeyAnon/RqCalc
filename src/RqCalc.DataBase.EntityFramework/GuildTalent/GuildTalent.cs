using System.ComponentModel.DataAnnotations.Schema;

using RqCalc.DataBase.EntityFramework._Base;
using RqCalc.Domain._Base;
using RqCalc.Domain.GuildTalent;

namespace RqCalc.DataBase.EntityFramework.GuildTalent;

[Table("GuildTalent")]
public partial class GuildTalent : ImageDirectoryBase
{
    public virtual HashSet<GuildTalentBonus> Bonuses { get; set; } = null!;

    public virtual HashSet<GuildTalentVariable> Variables { get; set; } = null!;


    public virtual GuildTalentBranch Branch { get; set; } = null!;

    public virtual Image GrayImage { get; set; } = null!;



    public string? Description { get; set; }


    public bool Active { get; set; }

    public int OrderIndex { get; set; }


    [Column("GrayImage_Id")]
    public int? GrayImageId { get; set; }

    [Column("Branch_Id")]
    public int? BranchId { get; set; }
}

public partial class GuildTalent : IGuildTalent
{
    IReadOnlyCollection<IGuildTalentBonus> IBonusContainer<IGuildTalentBonus>.Bonuses => this.Bonuses;

    IGuildTalentBranch IGuildTalent.Branch => this.Branch;

    IReadOnlyCollection<IGuildTalentVariable> IGuildTalent.Variables => this.Variables;
        
    IImage IGuildTalent.GrayImage => this.GrayImage;
}