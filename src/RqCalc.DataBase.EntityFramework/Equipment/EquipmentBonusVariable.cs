using System.ComponentModel.DataAnnotations.Schema;

using RqCalc.DataBase.EntityFramework._Base;

namespace RqCalc.DataBase.EntityFramework.Equipment;

[Table("EquipmentBonusVariable")]
public class EquipmentBonusVariable : PersistentDomainObjectBase
{
    public virtual EquipmentBonus EquipmentBonus { get; set; } = null!;


    public decimal Value { get; set; }
        
    public int Index { get; set; }


    [Column("EquipmentBonus_Id")]
    public int EquipmentBonusId { get; set; }
}