using System.ComponentModel.DataAnnotations.Schema;

using RqCalc.DataBase.EntityFramework._Base;
using RqCalc.Domain;
using RqCalc.Domain.Stamp;

namespace RqCalc.DataBase.EntityFramework.Stamp;

[Table("Stamp")]
public partial class Stamp : DirectoryBase
{
    public virtual HashSet<Buff> Buffs { get; set; } = null!;

    public virtual HashSet<StampVariant> Variants { get; set; } = null!;

    public virtual HashSet<StampEquipment> Equipments { get; set; } = null!;


    public bool IsLegacy { get; set; }
}

public partial class Stamp : IStamp
{
    IReadOnlyCollection<IBuff> IStamp.Buffs => this.Buffs;

    IReadOnlyCollection<IStampVariant> IStamp.Variants => this.Variants;

    IReadOnlyCollection<IStampEquipment> IStamp.Equipments => this.Equipments;
}