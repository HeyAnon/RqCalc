using System.ComponentModel.DataAnnotations.Schema;

using RqCalc.DataBase.EntityFramework._Base;
using RqCalc.Domain;
using RqCalc.Domain._Base;
using RqCalc.Domain.Card;
using RqCalc.Domain.Stamp;
using RqCalc.Domain.Talent;

namespace RqCalc.DataBase.EntityFramework;

[Table("Buff")]
public partial class Buff : ImageDirectoryBase
{
    public virtual HashSet<BuffBonus> Bonuses { get; set; } = null!;


    public virtual Class? Class { get; set; }

    public virtual Card.Card? Card { get; set; }

    public virtual Stamp.Stamp? Stamp { get; set; }


    public virtual Talent.Talent? TalentCondition { get; set; }
        
    public virtual Version? StartVersion { get; set; }

    public virtual Version? EndVersion { get; set; }

        
    public int Level { get; set; }

    public int MaxStackCount { get; set; }

    public bool AutoEnabled { get; set; }

    public bool IsNegate { get; set; }





    [Column("Card_Id")]
    public int? CardId { get; set; }

    [Column("Stamp_Id")]
    public int? StampId { get; set; }

    [Column("Class_Id")]
    public int? ClassId { get; set; }

    [Column("TalentCondition_Id")]
    public int? TalentConditionId { get; set; }

    [Column("StartVersion_Id")]
    public int? StartVersionId { get; set; }

    [Column("EndVersion_Id")]
    public int? EndVersionId { get; set; }
}

public partial class Buff : IBuff
{
    IReadOnlyCollection<IBuffBonus> IBonusContainer<IBuffBonus>.Bonuses => this.Bonuses;

    IClass? IBuff.Class => this.Class;

    ITalent? IBuff.TalentCondition => this.TalentCondition;

    IStamp? IBuff.Stamp => this.Stamp;

    ICard? IBuff.Card => this.Card;


    IVersion? IVersionObject.StartVersion => this.StartVersion;

    IVersion? IVersionObject.EndVersion => this.EndVersion;
}