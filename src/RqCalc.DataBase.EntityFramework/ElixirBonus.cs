using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.DataBase.EntityFramework
{
    [Table("ElixirBonus")]
    public partial class ElixirBonus : Bonus
    {
        public virtual Elixir Elixir { get; set; }


        [Key]
        [Column("Elixir_Id", Order = 0)]
        public int? ElixirId { get; set; }
    }
    
    public partial class ElixirBonus : IElixirBonus
    {
        
    }
}