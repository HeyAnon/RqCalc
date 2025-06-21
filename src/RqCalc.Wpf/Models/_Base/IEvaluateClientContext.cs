using RqCalc.Domain;

namespace RqCalc.Wpf.Models._Base
{
    public interface IEvaluateClientContext : IClientContext
    {
        IReadOnlyDictionary<TextTemplateVariableType, decimal> EvaluateStats { get; }
    }
}