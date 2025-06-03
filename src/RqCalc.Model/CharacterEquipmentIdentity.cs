using RqCalc.Domain._Base;
using RqCalc.Domain.Equipment;

namespace RqCalc.Model;

public record CharacterEquipmentIdentity(IEquipmentSlot Slot, int Index) : IIndexObject;