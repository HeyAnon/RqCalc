using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.DataBase.EntityFramework
{
    [Table("BuffBonus")]
    public partial class BuffBonus : Bonus
    {
        public virtual Buff Buff { get; set; }


        [Key]
        [Column("Buff_Id", Order = 0)]
        public int? BuffId { get; set; }
    }

    public partial class BuffBonus : IBuffBonus
    {

    }
}