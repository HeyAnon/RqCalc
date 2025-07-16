using System.ComponentModel.DataAnnotations.Schema;

using RqCalc.DataBase.EntityFramework._Base;
using RqCalc.DataBase.EntityFramework.Talent;
using RqCalc.Domain;
using RqCalc.Domain._Base;
using RqCalc.Domain.Talent;

namespace RqCalc.DataBase.EntityFramework;

[Table("Aura")]
public partial class Aura : ImageDirectoryBase
{
    public virtual HashSet<AuraBonus> Bonuses { get; set; } = null!;

    public virtual HashSet<TalentBonus> DependencyTalentBonuses { get; set; } = null!;


    public virtual Class Class { get; set; } = null!;

    public virtual Version? StartVersion { get; set; }

    public virtual Version? EndVersion { get; set; }


    public int Level { get; set; }



    [Column("Class_Id")]
    public int? ClassId { get; set; }


    [Column("StartVersion_Id")]
    public int? StartVersionId { get; set; }

    [Column("EndVersion_Id")]
    public int? EndVersionId { get; set; }
}

public partial class Aura : IAura
{
    IClass IAura.Class => this.Class;

    IReadOnlyCollection<IAuraBonus> IBonusContainer<IAuraBonus>.Bonuses => this.Bonuses;


    IVersion? IVersionObject.StartVersion => this.StartVersion;

    IVersion? IVersionObject.EndVersion => this.EndVersion;

    IReadOnlyCollection<ITalentBonus> IAura.DependencyTalentBonuses => this.DependencyTalentBonuses;
}