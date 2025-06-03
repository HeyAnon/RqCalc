using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.DataBase.EntityFramework
{
    [Table("TalentBranch")]
    public partial class TalentBranch : DirectoryBase
    {
        public virtual ICollection<Talent> Talents { get; set; }


        public virtual Class Class { get; set; }


        [Column("Class_Id")]
        public int? ClassId { get; set; }
    }

    public partial class TalentBranch : ITalentBranch
    {
        IClass IClassObject.Class => this.Class;

        IEnumerable<ITalent> ITalentBranch.Talents => this.Talents;
    }
}