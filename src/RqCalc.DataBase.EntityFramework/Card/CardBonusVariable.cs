using System.ComponentModel.DataAnnotations.Schema;

using RqCalc.DataBase.EntityFramework._Base;
using RqCalc.Domain.Card;

namespace RqCalc.DataBase.EntityFramework.Card;

[Table("CardBonusVariable")]
public partial class CardBonusVariable : PersistentDomainObjectBase
{
    public virtual HashSet<CardBonusVariableCondition> Conditions { get; set; } = null!;


    public virtual CardBonus CardBonus { get; set; } = null!;


    public int Value { get; set; }
        
    public int Index { get; set; }


    [Column("CardBonus_Id")]
    public int? CardBonusId { get; set; }
}

public partial class CardBonusVariable : ICardBonusVariable
{
    IReadOnlyCollection<ICardBonusVariableCondition> ICardBonusVariable.Conditions => this.Conditions;

    ICardBonus ICardBonusVariable.CardBonus => this.CardBonus;
}