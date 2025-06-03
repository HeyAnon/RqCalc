using System.ComponentModel.DataAnnotations.Schema;

using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.DataBase.EntityFramework
{
    [Table("CardType")]
    public partial class CardType : ImageDirectoryBase
    {
        public virtual Element Element { get; set; }


        public virtual Image ToolTipImage { get; set; }


        public virtual EquipmentClass MaxEquipmentClass { get; set; }


        [Column("ToolTipImage_Id")]
        public int? ToolTipImageId { get; set; }

        [Column("Element_Id")]
        public int? ElementId { get; set; }


        [Column("MaxEquipmentClass_Id")]
        public int? MaxEquipmentClassId { get; set; }
    }

    public partial class CardType : ICardType
    {
        IElement ICardType.Element => this.Element;
        
        IImage ICardType.ToolTipImage => this.ToolTipImage;

        IEquipmentClass ICardType.MaxEquipmentClass => this.MaxEquipmentClass;

        public bool HasToolTipImage => this.ToolTipImageId.HasValue;
    }
}