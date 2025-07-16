using System.ComponentModel.DataAnnotations.Schema;

using RqCalc.Domain.Equipment;

namespace RqCalc.DataBase.EntityFramework.Equipment;

[Table("EquipmentElixirSlot")]
public partial class EquipmentElixirSlot
{
    public virtual EquipmentElixir EquipmentElixir { get; set; } = null!;

    public virtual EquipmentSlot EquipmentSlot { get; set; } = null!;

    [Column("EquipmentElixir_Id")]
    public int EquipmentElixirId { get; set; }

    [Column("EquipmentSlot_Id")]
    public int EquipmentSlotId { get; set; }
}

public partial class EquipmentElixirSlot : IEquipmentElixirSlot
{
    IEquipmentElixir IEquipmentElixirSlot.EquipmentElixir => this.EquipmentElixir;

    IEquipmentSlot IEquipmentElixirSlot.EquipmentSlot => this.EquipmentSlot;
}