using System.ComponentModel.DataAnnotations.Schema;

using RqCalc.DataBase.EntityFramework._Base;
using RqCalc.Domain.Equipment;

namespace RqCalc.DataBase.EntityFramework.Equipment;

[Table("EquipmentElixirBonus")]
public partial class EquipmentElixirBonus : Bonus
{
    public virtual EquipmentElixir EquipmentElixir { get; set; } = null!;


    [Column("EquipmentElixir_Id")]
    public int EquipmentElixirId { get; set; }
}

public partial class EquipmentElixirBonus : IEquipmentElixirBonus
{
    IEquipmentElixir IEquipmentElixirBonus.EquipmentElixir => this.EquipmentElixir;
}