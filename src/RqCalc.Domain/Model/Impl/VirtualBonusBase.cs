using Framework.Core;
using RqCalc.Domain.Persistent._Base._Blocks;
using RqCalc.Domain.Persistent.BonusType;

namespace RqCalc.Domain.Model.Impl;

public class VirtualBonusBase : IBonusBase
{
    public IBonusType Type { get; set; }

    public List<decimal> Variables { get; set; }


    public override string ToString()
    {
        return $"BonusType: {this.Type.Template} | Values: {this.Variables.Join(", ")}";
    }

    IReadOnlyList<decimal> IBonusBase.Variables => this.Variables;
}