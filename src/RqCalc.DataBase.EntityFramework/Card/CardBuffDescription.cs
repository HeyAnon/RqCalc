using System.ComponentModel.DataAnnotations.Schema;

using RqCalc.Domain.Card;

namespace RqCalc.DataBase.EntityFramework.Card;

[Table("CardBuffDescription")]
public partial class CardBuffDescription : BuffElement
{
    public virtual Card Card { get; set; } = null!;
        

    [Column("Card_Id")]
    public int? CardId { get; set; }
}

public partial class CardBuffDescription : ICardBuffDescription
{
    ICard ICardBuffDescription.Card => this.Card;
}