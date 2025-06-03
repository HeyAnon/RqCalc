using System.ComponentModel.DataAnnotations.Schema;

namespace Anon.RQ_Calc.DataBase.EntityFramework
{
    [Table("EquipmentBonusVariable")]
    public partial class EquipmentBonusVariable : PersistentDomainObjectBase
    {
        public virtual EquipmentBonus EquipmentBonus { get; set; }


        public decimal Value { get; set; }
        
        public int Index { get; set; }


        [Column("EquipmentBonus_Id")]
        public int? EquipmentBonusId { get; set; }
    }
}