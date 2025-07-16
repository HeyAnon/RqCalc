namespace RqCalc.Application;

public static class DecimalExtensions
{
    public static decimal Normalize(this decimal value) => value / 1.0000000000000000000000000000m;
}