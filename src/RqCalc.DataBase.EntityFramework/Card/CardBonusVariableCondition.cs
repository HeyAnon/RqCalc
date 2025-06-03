using System.ComponentModel.DataAnnotations.Schema;

using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.DataBase.EntityFramework
{
    [Table("CardBonusVariableCondition")]
    public partial class CardBonusVariableCondition : PersistentDomainObjectBase
    {
        public virtual EquipmentType EquipmentType { get; set; }

        public virtual CardBonusVariable CardBonusVariable { get; set; }



        public bool? IsSingleHandWeapon { get; set; }


        [Column("EquipmentType_Id")]
        public int? EquipmentTypeId { get; set; }

        [Column("CardBonusVariable_Id")]
        public int? CardBonusVariableId { get; set; }
    }

    public partial class CardBonusVariableCondition : ICardBonusVariableCondition
    {
        ICardBonusVariable ICardBonusVariableCondition.CardBonusVariable => this.CardBonusVariable;

        IEquipmentType ICardBonusVariableCondition.EquipmentType => this.EquipmentType;
    }
}