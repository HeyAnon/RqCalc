using RqCalc.Domain.CollectedStatistic;

namespace RqCalc.Domain._Extensions;

public static class CollectedItemExtensions
{
    public static bool IsAllowed(this ICollectedItem collectedItem, IGender gender, IVersion version)
    {
        if (collectedItem == null) throw new ArgumentNullException(nameof(collectedItem));
        if (gender == null) throw new ArgumentNullException(nameof(gender));
        if (version == null) throw new ArgumentNullException(nameof(version));

        return (collectedItem.Gender == null || collectedItem.Gender == gender) && collectedItem.Contains(version);
    }
}