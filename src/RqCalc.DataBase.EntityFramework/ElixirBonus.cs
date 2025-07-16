using System.ComponentModel.DataAnnotations.Schema;

using RqCalc.DataBase.EntityFramework._Base;
using RqCalc.Domain;

namespace RqCalc.DataBase.EntityFramework;

[Table("ElixirBonus")]
public partial class ElixirBonus : Bonus
{
    public virtual Elixir Elixir { get; set; } = null!;


    [Column("Elixir_Id")]
    public int ElixirId { get; set; }
}
    
public partial class ElixirBonus : IElixirBonus
{
        
}