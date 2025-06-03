using RqCalc.Domain.Persistent._Base._Blocks;

namespace RqCalc.Domain.Model;

public interface ITextTemplate
{
    IReadOnlyList<ITextTemplateVariableBase> Variables { get; }

    string Header { get; }

    string Message { get; }
}