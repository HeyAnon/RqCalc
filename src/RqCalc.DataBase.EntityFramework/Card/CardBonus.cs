using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

using Framework.Core;

using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.DataBase.EntityFramework
{
    [Table("CardBonus")]
    public partial class CardBonus : PersistentDomainObjectBase
    {
        public virtual ICollection<CardBonusVariable> Variables { get; set; }

        public virtual Card Card { get; set; }

        public virtual BonusType Type { get; set; }

        public virtual Card RequiredCard { get; set; }

        public virtual Card NegateCard { get; set; }

        public virtual Card MultiplyEffectCard { get; set; }

        public virtual CardSet RequiredSet { get; set; }



        public virtual int RequiredSetSize { get; set; }

        public int OrderIndex { get; set; }

        public int? UpgConditionVariable { get; set; }

        public int? UpgLevelStepVariable { get; set; }


        [Column("Card_Id")]
        public int? CardId { get; set; }

        [Column("RequiredCard_Id")]
        public int? RequiredCardId { get; set; }

        [Column("NegateCard_Id")]
        public int? NegateCardId { get; set; }

        [Column("MultiplyEffectCard_Id")]
        public int? MultiplyEffectCardId { get; set; }

        [Column("RequiredSet_Id")]
        public int? RequiredSetId { get; set; }


        [Column("Type_Id")]
        public int? TypeId { get; set; }
        

        public override string ToString()
        {
            return $"BonusType: {this.Type.Template} | Values: {this.Variables.Join(", ", v => $"{v.Value} [{v.Index}]")}";
        }
    }

    public partial class CardBonus : ICardBonus
    {   
        private readonly Lazy<CardUpgradeEquipmentInfo> _lazyInfo;


        public CardBonus()
        {
            this._lazyInfo = LazyHelper.Create(this.GetInfo);
        }


        public CardUpgradeEquipmentInfo UpgradeEquipmentInfo => this._lazyInfo.Value;


        IEnumerable<ICardBonusVariable> ICardBonus.Variables => this.Variables;

        ICard ICardBonus.Card => this.Card;

        ICard ICardBonus.RequiredCard => this.RequiredCard;

        ICard ICardBonus.NegateCard => this.NegateCard;

        ICard ICardBonus.MultiplyEffectCard => this.MultiplyEffectCard;

        ICardSet ICardBonus.RequiredSet => this.RequiredSet;

        IBonusType Framework.Persistent.ITypeObject<IBonusType>.Type => this.Type;

        private CardUpgradeEquipmentInfo GetInfo()
        {
            if (this.UpgConditionVariable != null && this.UpgLevelStepVariable != null)
            {
                return new CardUpgradeEquipmentInfo(this.UpgConditionVariable.Value, this.UpgLevelStepVariable.Value);
            }
            else if (this.UpgConditionVariable == null && this.UpgLevelStepVariable == null)
            {
                return null;
            }
            else
            {
                throw new Exception("Invalid UpgradeEquipmentInfo");
            }
        }
    }
}