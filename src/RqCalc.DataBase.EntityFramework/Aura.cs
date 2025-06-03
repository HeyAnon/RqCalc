using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.DataBase.EntityFramework
{
    [Table("Aura")]
    public partial class Aura : ImageDirectoryBase
    {
        public virtual ICollection<AuraBonus> Bonuses { get; set; }

        public virtual ICollection<TalentBonus> DependencyTalentBonuses { get; set; }


        public virtual Class Class { get; set; }
        
        public virtual Version StartVersion { get; set; }

        public virtual Version EndVersion { get; set; }


        public int Level { get; set; }



        [Column("Class_Id")]
        public int? ClassId { get; set; }


        [Column("StartVersion_Id")]
        public int? StartVersionId { get; set; }

        [Column("EndVersion_Id")]
        public int? EndVersionId { get; set; }
    }

    public partial class Aura : IAura
    {
        IClass IClassObject.Class => this.Class;

        IEnumerable<IAuraBonus> IBonusContainer<IAuraBonus>.Bonuses => this.Bonuses;


        IVersion IVersionObject.StartVersion => this.StartVersion;

        IVersion IVersionObject.EndVersion => this.EndVersion;

        IEnumerable<ITalentBonus> IAura.DependencyTalentBonuses => this.DependencyTalentBonuses;
    }
}