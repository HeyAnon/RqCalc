using RqCalc.Domain.Persistent._Base._Blocks;
using RqCalc.Domain.Persistent.Equipment;

namespace RqCalc.Domain.Persistent.Card;

public interface ICardType : IImageDirectoryBase
{
    IElement Element { get; }

    IEquipmentClass MaxEquipmentClass { get; }

    IImage ToolTipImage { get; }

    bool HasToolTipImage { get; }
}