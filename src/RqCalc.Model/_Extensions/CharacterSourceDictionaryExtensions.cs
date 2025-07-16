using RqCalc.Domain.Equipment;

namespace RqCalc.Model._Extensions;

public static class CharacterSourceDictionaryExtensions
{
    public static TValue? GetValueOrDefault<TValue>(this IReadOnlyDictionary<CharacterEquipmentIdentity, TValue> dict, IEquipmentSlot slot, int index)
        where TValue : ICharacterEquipmentData
    {
        if (dict == null) throw new ArgumentNullException(nameof(dict));
        if (slot == null) throw new ArgumentNullException(nameof(slot));

        return dict.GetValueOrDefault(new CharacterEquipmentIdentity (slot, index));
    }
}