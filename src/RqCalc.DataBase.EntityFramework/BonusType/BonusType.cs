using System.ComponentModel.DataAnnotations.Schema;

using Framework.Core;

using RqCalc.DataBase.EntityFramework._Base;
using RqCalc.Domain;
using RqCalc.Domain.BonusType;

namespace RqCalc.DataBase.EntityFramework.BonusType;

[Table("BonusType")]
public partial class BonusType : PersistentDomainObjectBase
{
    public virtual HashSet<BonusTypeVariable> Variables { get; set; } = null!;

    public virtual HashSet<BonusTypeStat> Stats { get; set; } = null!;


    public bool IsSingle { get; set; }

    public string Template { get; set; } = null!;


    public decimal? StampQualityMinCoef { get; set; }

    public decimal? StampQualityMaxCoef { get; set; }


    public override string ToString() => this.Template;
}

public partial class BonusType : IBonusType
{
    private readonly Lazy<bool?> lazyIsMultiply;

    public BonusType() =>
        this.lazyIsMultiply = new Lazy<bool?>(() => this.Stats.IsEmpty()
                                                        ? null
                                                        : this.Stats.All(s => s.IsMultiply)
                                                            ? true
                                                            : this.Stats.All(s => !s.IsMultiply)
                                                                ? false
                                                                : null);

    IReadOnlyCollection<IBonusTypeStat> IBonusType.Stats => this.Stats;

    IReadOnlyCollection<IBonusTypeVariable> IBonusType.Variables => this.Variables;


    public StampQualityInfo? StampQuality => this.GetStampQuality();
        
    public bool? IsMultiply => this.lazyIsMultiply.Value;


    private StampQualityInfo? GetStampQuality()
    {
        if (this.StampQualityMinCoef == null && this.StampQualityMaxCoef == null)
        {
            return null;
        }
        else if (this.StampQualityMinCoef != null && this.StampQualityMaxCoef != null)
        {
            return new StampQualityInfo(this.StampQualityMinCoef.Value, this.StampQualityMaxCoef.Value);
        }
        else
        {
            throw new Exception("Invalid StampQuality");
        }
    }
}