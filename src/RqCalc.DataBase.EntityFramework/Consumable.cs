using System.ComponentModel.DataAnnotations.Schema;

using RqCalc.DataBase.EntityFramework._Base;
using RqCalc.Domain;
using RqCalc.Domain._Base;

namespace RqCalc.DataBase.EntityFramework;

[Table("Consumable")]
public partial class Consumable : ImageDirectoryBase
{
    public virtual HashSet<ConsumableBonus> Bonuses { get; set; } = null!;
}

public partial class Consumable : IConsumable
{
    IReadOnlyCollection<IConsumableBonus> IBonusContainer<IConsumableBonus>.Bonuses => this.Bonuses;
}