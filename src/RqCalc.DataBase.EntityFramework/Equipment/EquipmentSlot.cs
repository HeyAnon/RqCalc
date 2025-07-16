using System.ComponentModel.DataAnnotations.Schema;

using Framework.Core;

using RqCalc.DataBase.EntityFramework._Base;
using RqCalc.Domain;
using RqCalc.Domain.Equipment;

namespace RqCalc.DataBase.EntityFramework.Equipment;

[Table("EquipmentSlot")]
public partial class EquipmentSlot : ImageDirectoryBase
{
    public virtual HashSet<EquipmentType> Types { get; set; } = null!;


    public virtual HashSet<EquipmentSlot> PrimarySlots { get; set; } = null!;


    public virtual State? State { get; set; }

    public virtual EquipmentSlot? ExtraSlot { get; set; }


    public bool SharedStamp { get; set; }

    public int Count { get; set; }

    public bool? IsWeapon { get; set; }


    [Column("State_Id")]
    public int? StateId { get; set; }

    [Column("ExtraSlot_Id")]
    public int? ExtraSlotId { get; set; }
}

public partial class EquipmentSlot : IEquipmentSlot
{
    private readonly Lazy<EquipmentSlot?> lazyWeaponInfo;


    public EquipmentSlot() => this.lazyWeaponInfo = LazyHelper.Create(() => this.PrimarySlots.SingleOrDefault());

    public EquipmentSlot? PrimarySlot => this.lazyWeaponInfo.Value;


    IReadOnlyCollection<IEquipmentType> IEquipmentSlot.Types => this.Types;

    IState? IEquipmentSlot.State => this.State;

    IEquipmentSlot? IEquipmentSlot.ExtraSlot => this.ExtraSlot;

    IEquipmentSlot? IEquipmentSlot.PrimarySlot => this.PrimarySlot;
}