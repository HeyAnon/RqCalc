namespace RqCalc.Domain._Base;

public interface IBonusContainer<out TBonus>
{
    IReadOnlyList<TBonus> Bonuses { get; }
}