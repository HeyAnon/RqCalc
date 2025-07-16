namespace RqCalc.Domain._Base;

public interface IBonusContainer<out TBonus>
{
    IReadOnlyCollection<TBonus> Bonuses { get; }
}