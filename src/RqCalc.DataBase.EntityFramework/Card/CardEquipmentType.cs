using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.DataBase.EntityFramework
{
    [Table("CardEquipmentType")]
    public partial class CardEquipmentType : VersionObject
    {
        public virtual Card Card { get; set; }

        public virtual EquipmentType Type { get; set; }



        [Key]
        [Column("Card_Id", Order = 0)]
        public int? CardId { get; set; }

        [Key]
        [Column("Type_Id", Order = 1)]
        public int? TypeId { get; set; }
    }

    public partial class CardEquipmentType : ICardEquipmentType
    {
        ICard ICardEquipmentType.Card => this.Card;

        IEquipmentType ICardEquipmentType.Type => this.Type;
    }
}