using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.DataBase.EntityFramework
{
    [Table("CardBonusVariable")]
    public partial class CardBonusVariable : PersistentDomainObjectBase
    {
        public virtual ICollection<CardBonusVariableCondition> Conditions { get; set; }


        public virtual CardBonus CardBonus { get; set; }


        public int Value { get; set; }
        
        public int Index { get; set; }


        [Column("CardBonus_Id")]
        public int? CardBonusId { get; set; }
    }

    public partial class CardBonusVariable : ICardBonusVariable
    {
        IEnumerable<ICardBonusVariableCondition> ICardBonusVariable.Conditions => this.Conditions;

        ICardBonus ICardBonusVariable.CardBonus => this.CardBonus;
    }
}