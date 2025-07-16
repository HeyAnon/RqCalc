using RqCalc.Domain;

namespace RqCalc.Wpf.Models._Base;

public interface IEvaluateClientContext
{
    IReadOnlyDictionary<TextTemplateVariableType, decimal> EvaluateStats { get; }
}