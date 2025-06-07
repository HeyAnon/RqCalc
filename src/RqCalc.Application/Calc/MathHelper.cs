namespace RqCalc.Application.Calc;

public static class MathHelper
{
    public static decimal ArmorByLevel(int level, int maxLevel)
    {
        return 10040 * GetBaseLevelCoeff(level) / GetBaseLevelCoeff(maxLevel);
    }

    public static decimal AttackByLevel(int level)
    {
        return 340 * GetBaseLevelCoeff(level) - 7;
    }

    public static decimal GetBaseLevelCoeff(int level)
    {
        return (decimal)Math.Pow(Math.E, 0.05 * level);
    }
        
    public static decimal GetStampValue(decimal baseValue, int internalLevel, decimal minCoef, decimal maxCoef, int minLevel, int maxLevel)
    {
        var period = (maxCoef - minCoef) / (maxLevel - minLevel);

        var delta_level = Math.Max(0, internalLevel - minLevel);

        var r_points = (minCoef + period * delta_level) * baseValue;

        var c_points = Math.Ceiling(r_points);

        if (c_points - r_points < 0.98M)
            return c_points;
        else
            return Math.Truncate(r_points);
    }
}