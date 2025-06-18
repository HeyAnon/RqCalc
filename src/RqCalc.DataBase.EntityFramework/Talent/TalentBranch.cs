using System.ComponentModel.DataAnnotations.Schema;

using RqCalc.DataBase.EntityFramework._Base;
using RqCalc.Domain;
using RqCalc.Domain.Talent;

namespace RqCalc.DataBase.EntityFramework.Talent;

[Table("TalentBranch")]
public partial class TalentBranch : DirectoryBase
{
    public virtual HashSet<Talent> Talents { get; set; } = null!;


    public virtual Class Class { get; set; } = null!;


    [Column("Class_Id")]
    public int? ClassId { get; set; }
}

public partial class TalentBranch : ITalentBranch
{
    IClass ITalentBranch.Class => this.Class;

    IReadOnlyCollection<ITalent> ITalentBranch.Talents => this.Talents;
}