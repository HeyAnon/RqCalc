using System.ComponentModel.DataAnnotations.Schema;

using RqCalc.DataBase.EntityFramework._Base;
using RqCalc.Domain.Card;

namespace RqCalc.DataBase.EntityFramework.Card;

[Table("CardSet")]
public partial class CardSet : DirectoryBase
{
}

public partial class CardSet : ICardSet
{
}