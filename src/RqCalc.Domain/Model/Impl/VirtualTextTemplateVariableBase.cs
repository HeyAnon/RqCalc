using RqCalc.Core;
using RqCalc.Domain.Persistent._Base._Blocks;

namespace RqCalc.Domain.Model.Impl;

public class VirtualTextTemplateVariableBase : ITextTemplateVariableBase
{
    public decimal Value { get; set; }

    public TextTemplateVariableType Type { get; set; }
}