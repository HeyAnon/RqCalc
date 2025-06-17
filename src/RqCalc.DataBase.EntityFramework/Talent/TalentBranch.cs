using System.ComponentModel.DataAnnotations.Schema;
using RqCalc.DataBase.EntityFramework._Base;
using RqCalc.Domain;
using RqCalc.Domain.Talent;

namespace RqCalc.DataBase.EntityFramework.Talent
{
    [Table("TalentBranch")]
    public partial class TalentBranch : DirectoryBase
    {
        public virtual HashSet<Talent> Talents { get; set; }


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