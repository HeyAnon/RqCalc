using Framework.Core;

namespace RqCalc.Wpf._Extensions;

public static class EnumerableExtensions
{
    public static void ForeachS<TSource, TOther>(this IEnumerable<TSource> source, IEnumerable<TOther> other, Action<TSource, TOther> action)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (other == null) throw new ArgumentNullException(nameof(other));
        if (action == null) throw new ArgumentNullException(nameof(action));


        source.ZipStrong(other, (s, o) => new { S = s, O = o}).Foreach(pair => action(pair.S, pair.O));
    }
}