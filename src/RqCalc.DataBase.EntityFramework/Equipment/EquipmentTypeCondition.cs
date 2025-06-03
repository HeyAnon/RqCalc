using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Anon.RQ_Calc.Domain;

using Framework.Persistent;

namespace Anon.RQ_Calc.DataBase.EntityFramework
{
    [Table("EquipmentTypeCondition")]
    public partial class EquipmentTypeCondition
    {
        public virtual EquipmentType Type { get; set; }

        public virtual Class Class { get; set; }


        [Key]
        [Column("Type_Id", Order = 0)]
        public int? TypeId { get; set; }

        [Key]
        [Column("Class_Id", Order = 1)]
        public int? ClassId { get; set; }
    }

    public partial class EquipmentTypeCondition : IEquipmentTypeCondition
    {
        IEquipmentType ITypeObject<IEquipmentType>.Type => this.Type;

        IClass IClassObject.Class => this.Class;
    }
}