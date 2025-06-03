using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.DataBase.EntityFramework
{
    [Table("CollectedItemBonus")]
    public partial class CollectedItemBonus : Bonus
    {
        public virtual CollectedItem CollectedItem { get; set; }
        

        [Key]
        [Column("CollectedItem_Id", Order = 0)]
        public int CollectedItemId { get; set; }
    }

    public partial class CollectedItemBonus : ICollectedItemBonus
    {
    }
}