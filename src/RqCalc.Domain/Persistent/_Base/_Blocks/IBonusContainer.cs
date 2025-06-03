namespace RqCalc.Domain.Persistent._Base._Blocks;

public interface IBonusContainer<out TBonus>
{
    IEnumerable<TBonus> Bonuses { get; }
}

//public interface IBonusContainer : IBonusContainer<IBonus>
//{
        
//}