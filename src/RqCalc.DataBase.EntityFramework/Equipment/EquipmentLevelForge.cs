using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RqCalc.Domain.Equipment;

namespace RqCalc.DataBase.EntityFramework.Equipment
{
    [Table("EquipmentLevelForge")]
    public partial class EquipmentLevelForge
    {
        [Key]
        [Column(Order = 0)]
        public int Level { get; set; }

        [Key]
        [Column(Order = 1)]
        public int EquipmentLevel { get; set; }

        public int Hp { get; set; }
    }

    public partial class EquipmentLevelForge : IEquipmentLevelForge
    {

    }
}