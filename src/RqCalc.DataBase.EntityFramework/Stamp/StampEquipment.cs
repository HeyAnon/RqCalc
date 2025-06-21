using System.ComponentModel.DataAnnotations.Schema;

using RqCalc.DataBase.EntityFramework.Equipment;
using RqCalc.Domain.Equipment;
using RqCalc.Domain.Stamp;

namespace RqCalc.DataBase.EntityFramework.Stamp;

[Table("StampEquipment")]
public partial class StampEquipment : VersionObject
{
    public virtual EquipmentType Type { get; set; } = null!;

    public virtual Stamp Stamp { get; set; } = null!;


    [Column("Type_Id")]
    public int TypeId { get; set; }

    [Column("Stamp_Id")]
    public int StampId { get; set; }
}

public partial class StampEquipment : IStampEquipment
{
    IStamp IStampEquipment.Stamp => this.Stamp;

    IEquipmentType Framework.Persistent.ITypeObject<IEquipmentType>.Type => this.Type;
}