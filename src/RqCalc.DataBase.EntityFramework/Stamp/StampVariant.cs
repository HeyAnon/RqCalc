using System.ComponentModel.DataAnnotations.Schema;

using RqCalc.DataBase.EntityFramework._Base;
using RqCalc.Domain._Base;
using RqCalc.Domain.Stamp;

namespace RqCalc.DataBase.EntityFramework.Stamp;

[Table("StampVariant")]
public partial class StampVariant : PersistentDomainObjectBase
{
    public virtual HashSet<StampVariantBonus> Bonuses { get; set; } = null!;


    public virtual Stamp Stamp { get; set; } = null!;

    public virtual StampColor Color { get; set; } = null!;


    [Column("Stamp_Id")]
    public int? StampId { get; set; }

    [Column("Color_Id")]
    public int? ColorId { get; set; }
}

public partial class StampVariant : IStampVariant
{
    IReadOnlyCollection<IStampVariantBonus> IBonusContainer<IStampVariantBonus>.Bonuses => this.Bonuses;

    IStamp IStampVariant.Stamp => this.Stamp;

    IStampColor IStampVariant.Color => this.Color;
}