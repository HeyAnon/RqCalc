using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.DataBase.EntityFramework
{
    [Table("Buff")]
    public partial class Buff : ImageDirectoryBase
    {
        public virtual ICollection<BuffBonus> Bonuses { get; set; }


        public virtual Class Class { get; set; }

        public virtual Card Card { get; set; }

        public virtual Stamp Stamp { get; set; }


        public virtual Talent TalentCondition { get; set; }
        
        public virtual Version StartVersion { get; set; }

        public virtual Version EndVersion { get; set; }

        
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
        IEnumerable<IBuffBonus> IBonusContainer<IBuffBonus>.Bonuses => this.Bonuses;

        IClass IClassObject.Class => this.Class;

        ITalent IBuff.TalentCondition => this.TalentCondition;

        IStamp IBuff.Stamp => this.Stamp;

        ICard IBuff.Card => this.Card;


        IVersion IVersionObject.StartVersion => this.StartVersion;

        IVersion IVersionObject.EndVersion => this.EndVersion;
    }
}