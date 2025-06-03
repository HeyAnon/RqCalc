using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.DataBase.EntityFramework
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