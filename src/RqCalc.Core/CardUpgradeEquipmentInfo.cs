namespace RqCalc.Core;

public class CardUpgradeEquipmentInfo
{
    public CardUpgradeEquipmentInfo(int condition, int step)
    {
        if (condition < 0) throw new ArgumentOutOfRangeException(nameof(condition));
        if (step < 0) throw new ArgumentOutOfRangeException(nameof(step));

        this.Condition = condition;
        this.Step = step;
    }


    public int Condition { get; private set; }

    public int Step { get; private set; }
}