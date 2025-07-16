using RqCalc.Domain._Base;
using RqCalc.Domain.Card;
using RqCalc.Domain.Equipment;
using RqCalc.Domain.Stamp;

namespace RqCalc.Model;

public interface ICharacterEquipmentData : IActiveObject
{
    IEquipment Equipment { get; }
 
       
    IReadOnlyList<ICard?> Cards { get; }

    IStampVariant? StampVariant { get; }


    int Upgrade { get; }

    IEquipmentElixir? Elixir { get; }
}
