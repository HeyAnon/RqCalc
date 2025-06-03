using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.DataBase.EntityFramework
{
    [Table("GuildTalentVariable")]
    public partial class GuildTalentVariable
    {
        public virtual GuildTalent Talent { get; set; }


        public decimal Value { get; set; }


        [Key]
        [Column(Order = 1)]
        public int Index { get; set; }

        [Key]
        [Column(Order = 2)]
        public int Points { get; set; }

        [Key]
        [Column("Talent_Id", Order = 0)]
        public int? TalentId { get; set; }
    }

    public partial class GuildTalentVariable : IGuildTalentVariable
    {
        IGuildTalent IGuildTalentVariable.Talent => this.Talent;
    }
}