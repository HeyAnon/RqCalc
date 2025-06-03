using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.DataBase.EntityFramework
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