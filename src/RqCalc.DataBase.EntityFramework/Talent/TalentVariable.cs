using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.DataBase.EntityFramework
{
    [Table("TalentVariable")]
    public partial class TalentVariable
    {
        public virtual Talent Talent { get; set; }


        public decimal Value { get; set; }

        public TextTemplateVariableType Type { get; set; }

        [Key]
        [Column(Order = 0)]
        public int Index { get; set; }


        [Key]
        [Column("Talent_Id", Order = 1)]
        public int? TalentId { get; set; }
    }

    public partial class TalentVariable : ITalentVariable
    {
        ITalent ITalentVariable.Talent => this.Talent;
    }
}