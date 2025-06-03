using System.ComponentModel.DataAnnotations.Schema;

using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.DataBase.EntityFramework
{
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
}