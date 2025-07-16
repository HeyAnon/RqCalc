using RqCalc.Domain;

namespace RqCalc.Model._Extensions;

public static class CharacterSourceExtensions
{
    public static IEnumerable<IBuff> GetCardBuffs(this ICharacterSource characterSource)
    {
        var request = from eData in characterSource.Equipments.Values

            from card in eData.Cards
                          
            where card != null
                          
            from buff in card.Buffs
                          
            select buff;

        return request.Distinct();
    }

    public static IEnumerable<IBuff> GetStampBuffs(this ICharacterSource characterSource)
    {
        var request = from eData in characterSource.Equipments.Values

            where eData.StampVariant != null
                          
            from buff in eData.StampVariant.Stamp.Buffs
                          
            select buff;

        return request.Distinct();
    }
}