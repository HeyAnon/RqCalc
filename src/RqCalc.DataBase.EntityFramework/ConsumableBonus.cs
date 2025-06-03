using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.DataBase.EntityFramework
{
    [Table("ConsumableBonus")]
    public partial class ConsumableBonus : Bonus
    {
        public virtual Consumable Consumable { get; set; }


        [Key]
        [Column("Consumable_Id", Order = 0)]
        public int? ConsumableId { get; set; }
    }



    public partial class ConsumableBonus : IConsumableBonus
    {

    }
}