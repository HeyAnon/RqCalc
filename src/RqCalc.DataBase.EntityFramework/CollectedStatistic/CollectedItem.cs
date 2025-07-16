using System.ComponentModel.DataAnnotations.Schema;

using RqCalc.DataBase.EntityFramework._Base;
using RqCalc.Domain;
using RqCalc.Domain._Base;
using RqCalc.Domain.CollectedStatistic;

namespace RqCalc.DataBase.EntityFramework.CollectedStatistic;

[Table("CollectedItem")]
public partial class CollectedItem : PersistentDomainObjectBase
{
    public virtual HashSet<CollectedItemBonus> Bonuses { get; set; } = null!;

    public virtual Equipment.Equipment? Equipment { get; set; }

    public virtual Pet? Pet { get; set; }

    public virtual Trophy? Trophy { get; set; }

    public virtual CollectedGroup Group { get; set; } = null!;

    public virtual Version? NativeStartVersion { get; set; }

    public virtual Version? NativeEndVersion { get; set; }


    public virtual Version? StartVersion => this.NativeStartVersion ?? this.Equipment?.StartVersion;

    public virtual Version? EndVersion => this.NativeEndVersion ?? this.Equipment?.EndVersion;


    public Gender? Gender => this.Equipment?.Gender;

    public Image? Image => this.Equipment?.Image ?? this.Pet?.Image;

    public string Name => this.Equipment?.Name ?? this.Pet?.Name ?? this.Trophy!.Name;

        
    public int OrderIndex { get; set; }
        

    [Column("Equipment_Id")]
    public int? EquipmentId { get; set; }

    [Column("Pet_Id")]
    public int? PetId { get; set; }

    [Column("Trophy_Id")]
    public int? TrophyId { get; set; }

    [Column("Group_Id")]
    public int GroupId { get; set; }

    [Column("StartVersion_Id")]
    public int? NativeStartVersionId { get; set; }

    [Column("EndVersion_Id")]
    public int? NativeEndVersionId { get; set; }


    public override string ToString() => this.Name;
}

public partial class CollectedItem : ICollectedItem
{
    IReadOnlyCollection<ICollectedItemBonus> IBonusContainer<ICollectedItemBonus>.Bonuses => this.Bonuses;

    IImage? IImageObject.Image => this.Image;

    IGender? ICollectedItem.Gender => this.Gender;

    ICollectedGroup ICollectedItem.Group => this.Group;

    IVersion? IVersionObject.StartVersion => this.StartVersion;

    IVersion? IVersionObject.EndVersion => this.EndVersion;
}