using RqCalc.Domain;
using RqCalc.Domain._Base;

namespace RqCalc.Model;

public class TextTemplateVariable : ITextTemplateVariable
{
    public decimal Value { get; set; }

    public TextTemplateVariableType Type { get; set; }

    public int Index { get; set; }
}