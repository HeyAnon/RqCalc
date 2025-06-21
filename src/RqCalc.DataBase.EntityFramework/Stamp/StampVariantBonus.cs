using System.ComponentModel.DataAnnotations.Schema;

using RqCalc.DataBase.EntityFramework._Base;
using RqCalc.Domain.Stamp;

namespace RqCalc.DataBase.EntityFramework.Stamp;

[Table("StampVariantBonus")]
public class StampVariantBonus : Bonus, IStampVariantBonus
{
    public virtual StampVariant StampVariant { get; set; } = null!;


    public int QualityValue { get; set; }


    [Column("StampVariant_Id")]
    public int StampVariantId { get; set; }
}