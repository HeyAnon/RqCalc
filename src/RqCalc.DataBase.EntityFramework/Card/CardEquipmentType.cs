using System.ComponentModel.DataAnnotations.Schema;

using RqCalc.DataBase.EntityFramework.Equipment;
using RqCalc.Domain.Card;
using RqCalc.Domain.Equipment;

namespace RqCalc.DataBase.EntityFramework.Card;

[Table("CardEquipmentType")]
public partial class CardEquipmentType : VersionObject
{
    public virtual Card Card { get; set; } = null!;

    public virtual EquipmentType Type { get; set; } = null!;

    [Column("Card_Id")]
    public int CardId { get; set; }

    [Column("Type_Id")]
    public int TypeId { get; set; }
}

public partial class CardEquipmentType : ICardEquipmentType
{
    ICard ICardEquipmentType.Card => this.Card;

    IEquipmentType ICardEquipmentType.Type => this.Type;
}