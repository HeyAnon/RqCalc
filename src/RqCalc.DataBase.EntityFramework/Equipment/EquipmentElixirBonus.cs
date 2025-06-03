using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.DataBase.EntityFramework
{
    [Table("EquipmentElixirBonus")]
    public partial class EquipmentElixirBonus : Bonus
    {
        public virtual EquipmentElixir EquipmentElixir { get; set; }


        [Key]
        [Column("EquipmentElixir_Id", Order = 0)]
        public int? EquipmentElixirId { get; set; }
    }

    public partial class EquipmentElixirBonus : IEquipmentElixirBonus
    {
        IEquipmentElixir IEquipmentElixirBonus.EquipmentElixir => this.EquipmentElixir;
    }
}