using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.DataBase.EntityFramework
{
    [Table("EquipmentTypeBonus")]
    public partial class EquipmentTypeBonus : BonusBase
    {
        public virtual EquipmentType EquipmentType { get; set; }


        [Key]
        [Column("Type_Id", Order = 1)]
        public override int TypeId
        {
            get { return base.TypeId; }
            set { base.TypeId = value; }
        }

        [Key]
        [Column("EquipmentType_Id", Order = 0)]
        public int? EquipmentTypeId { get; set; }
    }

    public partial class EquipmentTypeBonus : IEquipmentTypeBonus
    {
        
    }
}