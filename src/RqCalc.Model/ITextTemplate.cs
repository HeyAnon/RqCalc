using RqCalc.Domain._Base;

namespace RqCalc.Model;

public interface ITextTemplate
{
    IReadOnlyList<ITextTemplateVariableBase> Variables { get; }

    string Header { get; }

    string Message { get; }
}