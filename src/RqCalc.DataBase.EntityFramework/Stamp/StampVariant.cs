using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.DataBase.EntityFramework
{
    [Table("StampVariant")]
    public partial class StampVariant : PersistentDomainObjectBase
    {
        public virtual ICollection<StampVariantBonus> Bonuses { get; set; }


        public virtual Stamp Stamp { get; set; }

        public virtual StampColor Color { get; set; }


        [Column("Stamp_Id")]
        public int? StampId { get; set; }

        [Column("Color_Id")]
        public int? ColorId { get; set; }
    }

    public partial class StampVariant : IStampVariant
    {
        System.Collections.Generic.IEnumerable<IStampVariantBonus> IBonusContainer<IStampVariantBonus>.Bonuses => this.Bonuses;

        IStamp IStampVariant.Stamp => this.Stamp;

        IStampColor IStampVariant.Color => this.Color;
    }
}