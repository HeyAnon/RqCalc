using Framework.Persistent;
using RqCalc.Domain;

namespace RqCalc.Wpf._Extensions;

public static class StatExtensions
{
    public static string GetBindName(this IStat stat)
    {
        if (stat == null) throw new ArgumentNullException(nameof(stat));

        var typePrefix = stat.Type == StatType.Other ? "" : (int)stat.Type + "|";

        return typePrefix + stat.GetNameObj().Name;
    }

    public static IVisualIdentityObject GetNameObj(this IStat stat)
    {
        if (stat == null) throw new ArgumentNullException(nameof(stat));

        if (stat.Element != null)
        {
            return stat.Element;
        }
        else if (stat.Race != null)
        {
            return stat.Race;
        }
        else
        {
            return stat;
        }
    }
}
public static class Int32Extensions
{
    public static int MinMax(this int value, int min, int max) => Math.Max(Math.Min(value, max), min);
}
