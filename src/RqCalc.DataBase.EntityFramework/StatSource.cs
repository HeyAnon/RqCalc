using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Anon.RQ_Calc.DataBase.EntityFramework
{
    [Table("StatSource")]
    public class StatSource
    {
        public virtual Stat Stat { get; set; }

        public virtual Formula Formula { get; set; }


        [Key]
        [Column("Stat_Id", Order = 0)]
        public int? StatId { get; set; }

        [Key]
        [Column("Formula_Id", Order = 1)]
        public int? FormulaId { get; set; }
    }
}