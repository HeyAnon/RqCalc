using System.ComponentModel.DataAnnotations.Schema;

using Framework.Core;

using RqCalc.DataBase.EntityFramework._Base;
using RqCalc.Domain;
using RqCalc.Domain._Base;
using RqCalc.Domain.Equipment;

namespace RqCalc.DataBase.EntityFramework.Equipment;

[Table("EquipmentElixir")]
public partial class EquipmentElixir : ImageDirectoryBase
{
    public virtual HashSet<EquipmentElixirBonus> Bonuses { get; set; } = null!;

    public virtual HashSet<EquipmentElixirSlot> NativeSlots { get; set; } = null!;
        

    public virtual Version? StartVersion { get; set; }

    public virtual Version? EndVersion { get; set; }



    [Column("StartVersion_Id")]
    public int? StartVersionId { get; set; }

    [Column("EndVersion_Id")]
    public int? EndVersionId { get; set; }
}


public partial class EquipmentElixir : IEquipmentElixir
{
    private readonly Lazy<EquipmentSlot[]> lazySlots;

    public EquipmentElixir() => this.lazySlots = LazyHelper.Create(() => this.NativeSlots.ToArray(ns => ns.EquipmentSlot));

    public IReadOnlyCollection<IEquipmentSlot> Slots => this.lazySlots.Value; // For wpf binging


    IReadOnlyCollection<IEquipmentElixirBonus> IBonusContainer<IEquipmentElixirBonus>.Bonuses => this.Bonuses;

    IVersion? IVersionObject.StartVersion => this.StartVersion;

    IVersion? IVersionObject.EndVersion => this.EndVersion;
}