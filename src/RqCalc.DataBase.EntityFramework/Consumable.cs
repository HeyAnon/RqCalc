using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.DataBase.EntityFramework
{
    [Table("Consumable")]
    public partial class Consumable : ImageDirectoryBase
    {
        public virtual ICollection<ConsumableBonus> Bonuses { get; set; }
    }

    public partial class Consumable : IConsumable
    {
        IEnumerable<IConsumableBonus> IBonusContainer<IConsumableBonus>.Bonuses => this.Bonuses;
    }
}