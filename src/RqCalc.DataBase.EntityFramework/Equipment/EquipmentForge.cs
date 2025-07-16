using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using RqCalc.Domain.Equipment;

namespace RqCalc.DataBase.EntityFramework.Equipment;

[Table("EquipmentForge")]
public partial class EquipmentForge
{
    [Key]
    public int Level { get; set; }

    public decimal Attack { get; set; }
        
    public decimal Defense { get; set; }

    public int AllStatBonus { get; set; }
}

public partial class EquipmentForge : IEquipmentForge
{
      
}