namespace RqCalc.Core;

public class StampQualityInfo
{
    public StampQualityInfo(decimal minCoef, decimal maxCoef)
    {
        this.MinCoef = minCoef;
        this.MaxCoef = maxCoef;
    }

    public decimal MinCoef { get; }

    public decimal MaxCoef { get; }
}