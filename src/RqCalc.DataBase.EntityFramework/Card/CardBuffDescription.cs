using System.ComponentModel.DataAnnotations.Schema;

using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.DataBase.EntityFramework
{
    [Table("CardBuffDescription")]
    public partial class CardBuffDescription : BuffElement
    {
        public virtual Card Card { get; set; }
        

        [Column("Card_Id")]
        public int? CardId { get; set; }
    }

    public partial class CardBuffDescription : ICardBuffDescription
    {
        ICard ICardBuffDescription.Card => this.Card;
    }
}