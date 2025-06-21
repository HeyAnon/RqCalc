using System.ComponentModel.DataAnnotations.Schema;

using RqCalc.DataBase.EntityFramework._Base;
using RqCalc.Domain;

namespace RqCalc.DataBase.EntityFramework;

[Table("ConsumableBonus")]
public partial class ConsumableBonus : Bonus
{
    public virtual Consumable Consumable { get; set; } = null!;


    [Column("Consumable_Id")]
    public int ConsumableId { get; set; }
}

public partial class ConsumableBonus : IConsumableBonus
{

}