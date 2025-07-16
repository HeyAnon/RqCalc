using System.ComponentModel.DataAnnotations.Schema;

using RqCalc.DataBase.EntityFramework._Base;
using RqCalc.Domain.CollectedStatistic;

namespace RqCalc.DataBase.EntityFramework.CollectedStatistic;

[Table("CollectedItemBonus")]
public partial class CollectedItemBonus : Bonus
{
    public virtual CollectedItem CollectedItem { get; set; } = null!;
        

    [Column("CollectedItem_Id")]
    public int CollectedItemId { get; set; }
}

public partial class CollectedItemBonus : ICollectedItemBonus
{
}