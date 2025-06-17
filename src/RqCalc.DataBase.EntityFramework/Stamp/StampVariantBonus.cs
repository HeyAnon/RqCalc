using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RqCalc.DataBase.EntityFramework._Base;
using RqCalc.Domain.Stamp;

namespace RqCalc.DataBase.EntityFramework.Stamp
{
    [Table("StampVariantBonus")]
    public partial class StampVariantBonus : Bonus, IStampVariantBonus
    {
        public virtual StampVariant StampVariant { get; set; }


        public int QualityValue { get; set; }


        [Key]
        [Column("StampVariant_Id", Order = 0)]
        public int StampVariantId { get; set; }
    }
}