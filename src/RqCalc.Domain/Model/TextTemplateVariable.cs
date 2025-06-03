using RqCalc.Core;
using RqCalc.Domain.Persistent._Base._Blocks;

namespace RqCalc.Domain.Model;

public class TextTemplateVariable : ITextTemplateVariable
{
    public decimal Value { get; set; }

    public TextTemplateVariableType Type { get; set; }

    public int Index { get; set; }
}