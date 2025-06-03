using RqCalc.Domain.Persistent._Base;
using RqCalc.Domain.Persistent.Card;
using RqCalc.Domain.Persistent.Equipment;
using RqCalc.Domain.Persistent.Stamp;

namespace RqCalc.Domain.Model;

public interface ICharacterEquipmentData : IActiveObject
{
    IEquipment Equipment { get; }
 
       
    IReadOnlyList<ICard?> Cards { get; }

    IStampVariant? StampVariant { get; }


    int Upgrade { get; }

    IEquipmentElixir? Elixir { get; }
}