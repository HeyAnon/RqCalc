using RqCalc.Domain.Persistent._Base._Blocks;

namespace RqCalc.Domain.Model.Impl;

public class VirtualBonusVariable : IBonusVariable
{
    public int Value { get; set; }
        
    public int Index { get; set; }
}