using RqCalc.Domain.Persistent._Base._Blocks;

namespace RqCalc.Domain.Model.Impl;

public class VirtualBonusBaseContainer : IBonusContainer<IBonusBase>
{
    public VirtualBonusBaseContainer()
    {
        this.Bonuses = new List<IBonusBase>();
    }


    public List<IBonusBase> Bonuses { get; set; }


    IEnumerable<IBonusBase> IBonusContainer<IBonusBase>.Bonuses => this.Bonuses;
}