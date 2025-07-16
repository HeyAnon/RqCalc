using System.ComponentModel.DataAnnotations.Schema;

using RqCalc.DataBase.EntityFramework.Equipment;
using RqCalc.Domain.Card;
using RqCalc.Domain.Equipment;

namespace RqCalc.DataBase.EntityFramework.Card;

[Table("CardEquipmentSlot")]
public partial class CardEquipmentSlot : VersionObject
{
    public virtual Card Card { get; set; } = null!;

    public virtual EquipmentSlot Slot { get; set; } = null!;

    [Column("Card_Id")]
    public int CardId { get; set; }

    [Column("Slot_Id")]
    public int SlotId { get; set; }
}

public partial class CardEquipmentSlot : ICardEquipmentSlot
{
    ICard ICardEquipmentSlot.Card => this.Card;

    IEquipmentSlot ICardEquipmentSlot.Slot => this.Slot;
}