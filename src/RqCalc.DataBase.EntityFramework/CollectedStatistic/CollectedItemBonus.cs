using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using RqCalc.DataBase.EntityFramework._Base;
using RqCalc.Domain.CollectedStatistic;

namespace RqCalc.DataBase.EntityFramework.CollectedStatistic
{
    [Table("CollectedItemBonus")]
    public partial class CollectedItemBonus : Bonus
    {
        public virtual CollectedItem CollectedItem { get; set; } = null!;
        

        [Key]
        [Column("CollectedItem_Id", Order = 0)]
        public int CollectedItemId { get; set; }
    }

    public partial class CollectedItemBonus : ICollectedItemBonus
    {
    }
}