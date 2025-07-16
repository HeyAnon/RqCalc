using System.ComponentModel.DataAnnotations.Schema;

using RqCalc.Domain.Equipment;

namespace RqCalc.DataBase.EntityFramework.Equipment;

[Table("EquipmentLevelForge")]
public partial class EquipmentLevelForge
{
    public int Level { get; set; }
    
    public int EquipmentLevel { get; set; }

    public int Hp { get; set; }
}

public partial class EquipmentLevelForge : IEquipmentLevelForge
{

}