using System.ComponentModel.DataAnnotations.Schema;

using RqCalc.DataBase.EntityFramework._Base;
using RqCalc.DataBase.EntityFramework.Equipment;
using RqCalc.Domain.Card;
using RqCalc.Domain.Equipment;

namespace RqCalc.DataBase.EntityFramework.Card;

[Table("CardBonusVariableCondition")]
public partial class CardBonusVariableCondition : PersistentDomainObjectBase
{
    public virtual EquipmentType? EquipmentType { get; set; }

    public virtual CardBonusVariable CardBonusVariable { get; set; } = null!;



    public bool? IsSingleHandWeapon { get; set; }


    [Column("EquipmentType_Id")]
    public int? EquipmentTypeId { get; set; }

    [Column("CardBonusVariable_Id")]
    public int? CardBonusVariableId { get; set; }
}

public partial class CardBonusVariableCondition : ICardBonusVariableCondition
{
    ICardBonusVariable ICardBonusVariableCondition.CardBonusVariable => this.CardBonusVariable;

    IEquipmentType? ICardBonusVariableCondition.EquipmentType => this.EquipmentType;
}