using RqCalc.Domain._Base;

namespace RqCalc.DataBase.EntityFramework._Base;

public abstract partial class Bonus : BonusBase
{
    public int OrderIndex { get; set; }
}

public abstract partial class Bonus : IBonus
{

}