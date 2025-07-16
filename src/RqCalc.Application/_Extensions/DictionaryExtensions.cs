using RqCalc.Domain;

namespace RqCalc.Application._Extensions;

internal static class DictionaryExtensions
{
    //internal static Dictionary<IStat, decimal> SumStats(this IEnumerable<IReadOnlyDictionary<IStat, decimal>> dicts)
    //{
    //    if (dicts == null) throw new ArgumentNullException("dicts");

    //    return dicts.Aggregate(new Dictionary<IStat, decimal>(), (d1, d2) => d1.SumStats(d2, functions));
    //}

    //internal static Dictionary<IStat, decimal> MulStats(this IEnumerable<IReadOnlyDictionary<IStat, decimal>> dicts)
    //{
    //    if (dicts == null) throw new ArgumentNullException("dicts");

    //    return dicts.Aggregate(new Dictionary<IStat, decimal>(), (d1, d2) => d1.MulStats(d2, functions));
    //}

    //internal static Dictionary<IStat, decimal> SumStats(this IReadOnlyDictionary<IStat, decimal> d1, IReadOnlyDictionary<IStat, decimal> d2)
    //{
    //    if (d1 == null) throw new ArgumentNullException("d1");
    //    if (d2 == null) throw new ArgumentNullException("d2");

    //    return new[] { d1, d2 }.AggregateValues((stat, x, y) => x + y);
    //}

    //internal static Dictionary<IStat, decimal> MulStats(this IReadOnlyDictionary<IStat, decimal> d1, IReadOnlyDictionary<IStat, decimal> d2)
    //{
    //    if (d1 == null) throw new ArgumentNullException("d1");
    //    if (d2 == null) throw new ArgumentNullException("d2");

    //    return new[] { d1, d2 }.AggregateValues((stat, x, y) =>
    //    {
    //        return x * y;
    //    });
    //}

    internal static decimal ToPercentValue(this decimal value) => 1 + value / 100;

    internal static Dictionary<IStat, decimal> TryRound(this IReadOnlyDictionary<IStat, decimal> stats)
    {
        if (stats == null) throw new ArgumentNullException(nameof(stats));
            
        return stats.ToDictionary(pair => pair.Key, pair => pair.Key.IsEditable ? Math.Round(pair.Value) : pair.Value);
    }
}