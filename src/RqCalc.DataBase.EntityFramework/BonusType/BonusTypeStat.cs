using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.DataBase.EntityFramework
{
    [Table("BonusTypeStat")]
    public partial class BonusTypeStat : PersistentDomainObjectBase
    {
        public virtual ICollection<BonusTypeStatCondition> Conditions { get; set; }

        public virtual BonusType BonusType { get; set; }

        public virtual Stat Stat { get; set; }


        public bool IsMultiply { get; set; }

        public int VarIndex { get; set; }


        [Column("BonusType_Id")]
        public int? BonusTypeId { get; set; }

        [Column("Stat_Id")]
        public int? StatId { get; set; }
    }

    public partial class BonusTypeStat : IBonusTypeStat
    {
        IEnumerable<IBonusTypeStatCondition> IBonusTypeStat.Conditions => this.Conditions;

        IBonusType IBonusTypeStat.BonusType => this.BonusType;

        IStat IBonusTypeStat.Stat => this.Stat;
    }
}