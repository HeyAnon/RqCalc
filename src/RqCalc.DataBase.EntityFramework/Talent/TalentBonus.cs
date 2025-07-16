using System.ComponentModel.DataAnnotations.Schema;

using Framework.Core;

using RqCalc.DataBase.EntityFramework._Base;
using RqCalc.Domain;
using RqCalc.Domain._Base;
using RqCalc.Domain.BonusType;
using RqCalc.Domain.Talent;

namespace RqCalc.DataBase.EntityFramework.Talent;

[Table("TalentBonus")]
public partial class TalentBonus : PersistentDomainObjectBase
{
    public virtual Talent Talent { get; set; } = null!;

    public virtual Aura? AuraCondition { get; set; }

    public virtual Buff? BuffCondition { get; set; }

    public virtual BonusType.BonusType Type { get; set; } = null!;


    [Column("AuraCondition_Id")]
    public int? AuraConditionId { get; set; }

    [Column("BuffCondition_Id")]
    public int? BuffConditionId { get; set; }


    public decimal? SharedValue { get; set; }


    [Column("Talent_Id")]
    public int? TalentId { get; set; }
        

    public decimal Value { get; set; }



    [Column("Type_Id")]
    public virtual int TypeId { get; set; }


    public override string ToString() => $"BonusType: {this.Type.Template} | Value: {this.Value}";
}

public partial class TalentBonus : ITalentBonus
{
    private readonly Lazy<List<decimal>> lazyVariables;


    protected TalentBonus() => this.lazyVariables = LazyHelper.Create(() => new List<decimal> { this.Value });

    ITalent ITalentBonus.Talent => this.Talent;

    IAura? ITalentBonus.AuraCondition => this.AuraCondition;

    IBuff? ITalentBonus.BuffCondition => this.BuffCondition;

    IReadOnlyCollection<decimal> IBonusBase.Variables => this.lazyVariables.Value;

    IBonusType Framework.Persistent.ITypeObject<IBonusType>.Type => this.Type;
}