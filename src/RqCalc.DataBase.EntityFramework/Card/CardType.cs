using System.ComponentModel.DataAnnotations.Schema;

using RqCalc.DataBase.EntityFramework._Base;
using RqCalc.DataBase.EntityFramework.Equipment;
using RqCalc.Domain;
using RqCalc.Domain._Base;
using RqCalc.Domain.Card;
using RqCalc.Domain.Equipment;

namespace RqCalc.DataBase.EntityFramework.Card;

[Table("CardType")]
public partial class CardType : ImageDirectoryBase
{
    public virtual Element? Element { get; set; }


    public virtual Image? ToolTipImage { get; set; }


    public virtual EquipmentClass? MaxEquipmentClass { get; set; }


    [Column("ToolTipImage_Id")]
    public int? ToolTipImageId { get; set; }

    [Column("Element_Id")]
    public int? ElementId { get; set; }


    [Column("MaxEquipmentClass_Id")]
    public int? MaxEquipmentClassId { get; set; }
}

public partial class CardType : ICardType
{
    IElement? ICardType.Element => this.Element;
        
    IImage? ICardType.ToolTipImage => this.ToolTipImage;

    IEquipmentClass? ICardType.MaxEquipmentClass => this.MaxEquipmentClass;

    public bool HasToolTipImage => this.ToolTipImageId.HasValue;
}