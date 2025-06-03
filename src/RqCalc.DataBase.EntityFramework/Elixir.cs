using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.DataBase.EntityFramework
{
    [Table("Elixir")]
    public partial class Elixir : ImageDirectoryBase
    {
        public virtual ICollection<ElixirBonus> Bonuses { get; set; }


        public bool IsLegacy { get; set; }
    }

    public partial class Elixir : IElixir
    {
        IEnumerable<IElixirBonus> IBonusContainer<IElixirBonus>.Bonuses => this.Bonuses;
    }
}