using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RqCalc.Domain;
using RqCalc.Domain.Equipment;

namespace RqCalc.DataBase.EntityFramework.Equipment
{
    [Table("EquipmentCondition")]
    public partial class EquipmentCondition
    {
        public virtual Class Class { get; set; }

        public virtual Equipment Equipment { get; set; }


        [Key]
        [Column("Class_Id", Order = 1)]
        public int? ClassId { get; set; }

        [Key]
        [Column("Equipment_Id", Order = 0)]
        public int? EquipmentId { get; set; }
    }

    public partial class EquipmentCondition : IEquipmentCondition
    {
        IEquipment IEquipmentCondition.Equipment => this.Equipment;

        IClass IClassObject.Class => this.Class;
    }
}