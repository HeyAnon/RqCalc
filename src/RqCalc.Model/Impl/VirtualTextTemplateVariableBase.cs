using RqCalc.Domain;
using RqCalc.Domain._Base;

namespace RqCalc.Model.Impl;

public class VirtualTextTemplateVariableBase : ITextTemplateVariableBase
{
    public decimal Value { get; set; }

    public TextTemplateVariableType Type { get; set; }
}