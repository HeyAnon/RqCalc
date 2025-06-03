using RqCalc.Domain.Persistent._Base._Blocks;
using RqCalc.Domain.Persistent.Equipment;

namespace RqCalc.Domain.Model;

public record CharacterEquipmentIdentity(IEquipmentSlot Slot, int Index) : IIndexObject;