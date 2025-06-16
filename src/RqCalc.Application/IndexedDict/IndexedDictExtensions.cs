using Framework.Persistent;

namespace RqCalc.Application.IndexedDict;

public static class IndexedDictExtensions
{
    public static IIndexedDict<T> ToIndexedDict<T>(this IEnumerable<T> source)
        where T : class, IIdentityObject<int>
    {
        return source.ToIndexedDict(v => v.Id);
    }

    public static IIndexedDict<T> ToIndexedDict<T, TKey>(this IEnumerable<T> source, Func<T, TKey> orderSelector)
        where T : class
    {
        return new IndexedDict<T>(source.OrderBy(orderSelector));
    }

    public static IIndexedDict<T?> ToNullable<T>(this IIndexedDict<T> source)
        where T : class, IIdentityObject<int>
    {
        return new NullableIndexedDict<T>(source);
    }
}