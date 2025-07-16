using System.ComponentModel.DataAnnotations.Schema;

using RqCalc.DataBase.EntityFramework._Base;
using RqCalc.Domain;
using RqCalc.Domain.BonusType;

namespace RqCalc.DataBase.EntityFramework.BonusType;

[Table("BonusTypeStat")]
public partial class BonusTypeStat : PersistentDomainObjectBase
{
    public virtual HashSet<BonusTypeStatCondition> Conditions { get; set; } = null!;

    public virtual BonusType BonusType { get; set; } = null!;

    public virtual Stat Stat { get; set; } = null!;


    public bool IsMultiply { get; set; }

    public int VarIndex { get; set; }


    [Column("BonusType_Id")]
    public int? BonusTypeId { get; set; }

    [Column("Stat_Id")]
    public int? StatId { get; set; }
}

public partial class BonusTypeStat : IBonusTypeStat
{
    IReadOnlyCollection<IBonusTypeStatCondition> IBonusTypeStat.Conditions => this.Conditions;

    IBonusType IBonusTypeStat.BonusType => this.BonusType;

    IStat IBonusTypeStat.Stat => this.Stat;
}