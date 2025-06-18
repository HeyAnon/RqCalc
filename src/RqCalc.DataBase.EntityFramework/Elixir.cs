using System.ComponentModel.DataAnnotations.Schema;

using RqCalc.DataBase.EntityFramework._Base;
using RqCalc.Domain;
using RqCalc.Domain._Base;

namespace RqCalc.DataBase.EntityFramework;

[Table("Elixir")]
public partial class Elixir : ImageDirectoryBase
{
    public virtual HashSet<ElixirBonus> Bonuses { get; set; } = null!;


    public bool IsLegacy { get; set; }
}

public partial class Elixir : IElixir
{
    IReadOnlyCollection<IElixirBonus> IBonusContainer<IElixirBonus>.Bonuses => this.Bonuses;
}