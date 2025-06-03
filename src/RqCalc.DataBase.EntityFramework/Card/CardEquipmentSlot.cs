using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.DataBase.EntityFramework
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