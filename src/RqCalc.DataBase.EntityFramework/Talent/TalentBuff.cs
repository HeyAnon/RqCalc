using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.DataBase.EntityFramework
{
    [Table("TalentBuffDescription")]
    public partial class TalentBuffDescription : BuffElement
    {
        public virtual Talent Talent { get; set; }

        
        public bool IsPassive { get; set; }


        [Column("Talent_Id")]
        public int? TalentId { get; set; }
    }

    public partial class TalentBuffDescription : ITalentBuffDescription
    {
        ITalent ITalentBuffDescription.Talent => this.Talent;
    }
}