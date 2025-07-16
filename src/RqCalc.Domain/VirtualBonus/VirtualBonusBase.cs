using Framework.Core;

using RqCalc.Domain._Base;
using RqCalc.Domain.BonusType;

namespace RqCalc.Domain.VirtualBonus;

public class VirtualBonusBase : IBonusBase
{
    public IBonusType Type { get; set; }

    public List<decimal> Variables { get; set; }


    public override string ToString() => $"BonusType: {this.Type.Template} | Values: {this.Variables.Join(", ")}";

    IReadOnlyCollection<decimal> IBonusBase.Variables => this.Variables;
}