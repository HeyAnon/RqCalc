using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RqCalc.Domain.Equipment;

namespace RqCalc.DataBase.EntityFramework.Equipment
{
    [Table("EquipmentElixirSlot")]
    public partial class EquipmentElixirSlot
    {
        public virtual EquipmentElixir EquipmentElixir { get; set; }

        public virtual EquipmentSlot EquipmentSlot { get; set; }


        [Key]
        [Column("EquipmentElixir_Id", Order = 0)]
        public int? EquipmentElixirId { get; set; }

        [Key]
        [Column("EquipmentSlot_Id", Order = 1)]
        public int? EquipmentSlotId { get; set; }
    }

    public partial class EquipmentElixirSlot : IEquipmentElixirSlot
    {
        IEquipmentElixir IEquipmentElixirSlot.EquipmentElixir => this.EquipmentElixir;

        IEquipmentSlot IEquipmentElixirSlot.EquipmentSlot => this.EquipmentSlot;
    }
}