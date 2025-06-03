using Framework.Persistent;

namespace RqCalc.Domain._Extensions;

public static class EnumerableExtensions
{
    public static IOrderedEnumerable<T> OrderById<T>(this IEnumerable<T> source)
        where T : IIdentityObject<int>
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        return source.OrderBy(v => v.Id);
    }

    public static IOrderedEnumerable<KeyValuePair<TKey, TValue>> OrderById<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> source)
        where TKey : IIdentityObject<int>
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        return source.OrderBy(v => v.Key.Id);
    }
}