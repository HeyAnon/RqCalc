using System.ComponentModel.DataAnnotations.Schema;

using RqCalc.DataBase.EntityFramework._Base;
using RqCalc.Domain.Equipment;

namespace RqCalc.DataBase.EntityFramework.Equipment;

[Table("EquipmentClass")]
public partial class EquipmentClass : DirectoryBase
{
    public int Level { get; set; }

    public int? MaxUpgradeLevel { get; set; }

    public bool SharedStamp { get; set; }
}

public partial class EquipmentClass : IEquipmentClass
{

}