using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.DataBase.EntityFramework
{
    [Table("StampEquipment")]
    public partial class StampEquipment : VersionObject
    {
        public virtual EquipmentType Type { get; set; }
        
        public virtual Stamp Stamp { get; set; }


        [Key]
        [Column("Type_Id", Order = 0)]
        public int? TypeId { get; set; }

        [Key]
        [Column("Stamp_Id", Order = 1)]
        public int? StampId { get; set; }
    }

    public partial class StampEquipment : IStampEquipment
    {
        IStamp IStampEquipment.Stamp => this.Stamp;

        IEquipmentType Framework.Persistent.ITypeObject<IEquipmentType>.Type => this.Type;
    }
}