namespace RqCalc.Application.Calculation;

public static class MathHelper
{
    public static decimal ArmorByLevel(int level, int maxLevel) => 10040 * GetBaseLevelCoeff(level) / GetBaseLevelCoeff(maxLevel);

    public static decimal AttackByLevel(int level) => 340 * GetBaseLevelCoeff(level) - 7;

    public static decimal GetBaseLevelCoeff(int level) => (decimal)Math.Pow(Math.E, 0.05 * level);

    public static decimal GetStampValue(decimal baseValue, int internalLevel, decimal minCoef, decimal maxCoef, int minLevel, int maxLevel)
    {
        var period = (maxCoef - minCoef) / (maxLevel - minLevel);

        var deltaLevel = Math.Max(0, internalLevel - minLevel);

        var rPoints = (minCoef + period * deltaLevel) * baseValue;

        var cPoints = Math.Ceiling(rPoints);

        if (cPoints - rPoints < 0.98M)
            return cPoints;
        else
            return Math.Truncate(rPoints);
    }
}