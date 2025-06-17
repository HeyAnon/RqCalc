using System.ComponentModel.DataAnnotations.Schema;
using Framework.Core;
using RqCalc.DataBase.EntityFramework._Base;
using RqCalc.DataBase.EntityFramework.Equipment;
using RqCalc.Domain;
using RqCalc.Domain._Base;
using RqCalc.Domain.Card;
using RqCalc.Domain.Equipment;

namespace RqCalc.DataBase.EntityFramework.Card
{
    [Table("Card")]
    public partial class Card : DirectoryBase
    {
        public virtual HashSet<Buff> Buffs { get; set; }

        public virtual HashSet<CardBonus> Bonuses { get; set; }

        public virtual HashSet<CardEquipmentSlot> EquipmentSlots { get; set; }

        public virtual HashSet<CardEquipmentType> EquipmentTypes { get; set; }

        public virtual HashSet<CardBuffDescription> BuffDescriptions { get; set; }


        public virtual CardType Type { get; set; }

        public virtual EquipmentClass MinEquipmentClass { get; set; }

        public virtual CardSet Set { get; set; }


        public virtual Version StartVersion { get; set; }

        public virtual Version EndVersion { get; set; }


        public bool IsLegacy { get; set; }


        public string GroupKey { get; set; }

        public string GroupValue { get; set; }

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
        private readonly Lazy<CardGroupInfo> lazyCardGroup;

        public Card()
        {
            this.lazyCardGroup = LazyHelper.Create(() =>
            {
                var request = from groupKey in this.GroupKey.ToMaybe()

                              from groupValue in this.GroupValue.ToMaybe<string>()

                              from groupOrderKey in this.GroupOrderKey.ToMaybe()

                              select new CardGroupInfo(groupKey, groupValue, groupOrderKey);

                return request.GetValueOrDefault();
            });
        }


        public CardGroupInfo Group => this.lazyCardGroup.Value;


        IEnumerable<IBuff> ICard.Buffs => this.Buffs;

        IEnumerable<ICardEquipmentSlot> ICard.EquipmentSlots => this.EquipmentSlots;

        IEnumerable<ICardEquipmentType> ICard.EquipmentTypes => this.EquipmentTypes;


        IEnumerable<ICardBonus> IBonusContainer<ICardBonus>.Bonuses => this.Bonuses;


        ICardType Framework.Persistent.ITypeObject<ICardType>.Type => this.Type;


        IEnumerable<ICardBuffDescription> ICard.BuffDescriptions => this.BuffDescriptions;

        IEquipmentClass ICard.MinEquipmentClass => this.MinEquipmentClass;

        ICardSet ICard.Set => this.Set;


        IVersion IVersionObject.StartVersion => this.StartVersion;

        IVersion IVersionObject.EndVersion => this.EndVersion;
    }
}