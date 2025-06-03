using System.Collections.Generic;

namespace Anon.RQ_Calc.WPF
{
    public interface IEvaluateClientContext : IClientContext
    {
        IReadOnlyDictionary<TextTemplateVariableType, decimal> EvaluateStats { get; }
    }
}