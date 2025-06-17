using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RqCalc.DataBase.EntityFramework.Equipment;
using RqCalc.Domain.Card;
using RqCalc.Domain.Equipment;

namespace RqCalc.DataBase.EntityFramework.Card
{
    [Table("CardEquipmentSlot")]
    public partial class CardEquipmentSlot : VersionObject
    {
        public virtual Card Card { get; set; }

        public virtual EquipmentSlot Slot { get; set; }



        [Key]
        [Column("Card_Id", Order = 0)]
        public int? CardId { get; set; }

        [Key]
        [Column("Slot_Id", Order = 1)]
        public int? SlotId { get; set; }
    }

    public partial class CardEquipmentSlot : ICardEquipmentSlot
    {
        ICard ICardEquipmentSlot.Card => this.Card;

        IEquipmentSlot ICardEquipmentSlot.Slot => this.Slot;
    }
}