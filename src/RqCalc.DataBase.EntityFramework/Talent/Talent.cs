using System.ComponentModel.DataAnnotations.Schema;

using RqCalc.DataBase.EntityFramework._Base;
using RqCalc.Domain._Base;
using RqCalc.Domain.Talent;

namespace RqCalc.DataBase.EntityFramework.Talent;

[Table("Talent")]
public partial class Talent : ImageDirectoryBase
{
    public virtual HashSet<TalentBonus> Bonuses { get; set; } = null!;

    public virtual HashSet<TalentVariable> Variables { get; set; } = null!;

    public virtual HashSet<TalentBuffDescription> BuffDescriptions { get; set; } = null!;


    public virtual TalentBranch Branch { get; set; } = null!;

    public virtual Image GrayImage { get; set; } = null!;



    public string Description { get; set; } = null!;

    public int VIndex { get; set; }

    public int HIndex { get; set; }

    public bool Active { get; set; }

    public string? PassiveDescription { get; set; }

    public string? EquipmentCondition { get; set; }


    [Column("GrayImage_Id")]
    public int? GrayImageId { get; set; }

    [Column("Branch_Id")]
    public int? BranchId { get; set; }
}

public partial class Talent : ITalent
{
    IReadOnlyCollection<ITalentBonus> IBonusContainer<ITalentBonus>.Bonuses => this.Bonuses;

    ITalentBranch ITalent.Branch => this.Branch;

    IReadOnlyCollection<ITalentVariable> ITalent.Variables => this.Variables;

    IReadOnlyCollection<ITalentBuffDescription> ITalent.BuffDescriptions => this.BuffDescriptions;

    IImage ITalent.GrayImage => this.GrayImage;
}