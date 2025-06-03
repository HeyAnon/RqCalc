using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

using Anon.RQ_Calc.Domain;
using Framework.Core;

namespace Anon.RQ_Calc.DataBase.EntityFramework
{
    [Table("Card")]
    public partial class Card : DirectoryBase
    {
        public virtual ICollection<Buff> Buffs { get; set; }

        public virtual ICollection<CardBonus> Bonuses { get; set; }

        public virtual ICollection<CardEquipmentSlot> EquipmentSlots { get; set; }

        public virtual ICollection<CardEquipmentType> EquipmentTypes { get; set; }

        public virtual ICollection<CardBuffDescription> BuffDescriptions { get; set; }


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
        private readonly Lazy<CardGroupInfo> _lazyCardGroup;

        public Card()
        {
            this._lazyCardGroup = LazyHelper.Create(() =>
            {
                var request = from groupKey in this.GroupKey.ToMaybe()

                              from groupValue in this.GroupValue.ToMaybe<string>()

                              from groupOrderKey in this.GroupOrderKey.ToMaybe()

                              select new CardGroupInfo(groupKey, groupValue, groupOrderKey);

                return request.GetValueOrDefault();
            });
        }


        public CardGroupInfo Group => this._lazyCardGroup.Value;


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