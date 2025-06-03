using Framework.Persistent;

using RqCalc.Domain._Base;
using RqCalc.Domain.VirtualBonus;

namespace RqCalc.Domain._Extensions;

public static class BonusContainerExtensions
{
    public static IEnumerable<TBonus> GetOrderedBonuses<TBonus>(this IBonusContainer<TBonus> bonusContainer)
        where TBonus : IOrderObject<int>
    {
        if (bonusContainer == null) throw new ArgumentNullException(nameof(bonusContainer));

        return bonusContainer.Bonuses.OrderBy(bonus => bonus.OrderIndex);
    }

    public static IBonusBase ExpandShared<TBonus>(this TBonus bonus)
        where TBonus : class, IBonusBase, IValueObject<decimal>, ISharedContainer
    {
        if (bonus == null) throw new ArgumentNullException(nameof(bonus));

        return new VirtualBonusBase
        {
            Type = bonus.Type,

            Variables = [bonus.SharedValue ?? bonus.Value]
        };
    }
}