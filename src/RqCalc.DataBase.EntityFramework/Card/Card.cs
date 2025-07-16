using System.ComponentModel.DataAnnotations.Schema;

using Framework.Core;

using RqCalc.DataBase.EntityFramework._Base;
using RqCalc.DataBase.EntityFramework.Equipment;
using RqCalc.Domain;
using RqCalc.Domain._Base;
using RqCalc.Domain.Card;
using RqCalc.Domain.Equipment;

namespace RqCalc.DataBase.EntityFramework.Card;

[Table("Card")]
public partial class Card : DirectoryBase
{
    public virtual HashSet<Buff> Buffs { get; set; } = null!;

    public virtual HashSet<CardBonus> Bonuses { get; set; } = null!;

    public virtual HashSet<CardEquipmentSlot> EquipmentSlots { get; set; } = null!;

    public virtual HashSet<CardEquipmentType> EquipmentTypes { get; set; } = null!;

    public virtual HashSet<CardBuffDescription> BuffDescriptions { get; set; } = null!;


    public virtual CardType Type { get; set; } = null!;

    public virtual EquipmentClass? MinEquipmentClass { get; set; }

    public virtual CardSet? Set { get; set; }


    public virtual Version? StartVersion { get; set; }

    public virtual Version? EndVersion { get; set; }


    public bool IsLegacy { get; set; }


    public string? GroupKey { get; set; }

    public string? GroupValue { get; set; }

    public int? GroupOrderKey { get; set; }

    public bool? IsSingleHandWeapon { get; set; }

    public bool? IsMeleeWeapon { get; set; }

    [Column("Type_Id")]
    public int? TypeId { get; set; }

    [Column("Set_Id")]
    public int? SetId { get; set; }


    [Column("StartVersion_Id")]
    public int? StartVersionId { get; set; }

    [Column("EndVersion_Id")]
    public int? EndVersionId { get; set; }

    [Column("MinEquipmentClass_Id")]
    public int? MinEquipmentClassId { get; set; }
}

public partial class Card : ICard
{
    private readonly Lazy<CardGroupInfo?> lazyCardGroup;

    public Card() =>
        this.lazyCardGroup = LazyHelper.Create(() =>
                                               {
                                                   if (this.GroupKey == null || this.GroupValue == null || this.GroupOrderKey == null)
                                                   {
                                                       return null;
                                                   }
                                                   else
                                                   {
                                                       return new CardGroupInfo(this.GroupKey, this.GroupValue, this.GroupOrderKey.Value);
                                                   }
                                               });

    public CardGroupInfo? Group => this.lazyCardGroup.Value;


    IReadOnlyCollection<IBuff> ICard.Buffs => this.Buffs;

    IReadOnlyCollection<ICardEquipmentSlot> ICard.EquipmentSlots => this.EquipmentSlots;

    IReadOnlyCollection<ICardEquipmentType> ICard.EquipmentTypes => this.EquipmentTypes;


    IReadOnlyCollection<ICardBonus> IBonusContainer<ICardBonus>.Bonuses => this.Bonuses;


    ICardType Framework.Persistent.ITypeObject<ICardType>.Type => this.Type;


    IReadOnlyCollection<ICardBuffDescription> ICard.BuffDescriptions => this.BuffDescriptions;

    IEquipmentClass? ICard.MinEquipmentClass => this.MinEquipmentClass;

    ICardSet? ICard.Set => this.Set;


    IVersion? IVersionObject.StartVersion => this.StartVersion;

    IVersion? IVersionObject.EndVersion => this.EndVersion;
}