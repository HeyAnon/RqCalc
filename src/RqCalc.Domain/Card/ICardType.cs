using RqCalc.Domain._Base;
using RqCalc.Domain.Equipment;

namespace RqCalc.Domain.Card;

public interface ICardType : IImageDirectoryBase
{
    IElement? Element { get; }

    IEquipmentClass? MaxEquipmentClass { get; }

    IImage? ToolTipImage { get; }

    bool HasToolTipImage { get; }
}