using RqCalc.Domain;

namespace RqCalc.Application.Calculation;

public record BonusEvaluator(int Priority)
{
    public record ConstBonusEvaluator(int Priority, bool IsMultiply, IReadOnlyDictionary<IStat, decimal> Stats) : BonusEvaluator(Priority)
    {
        public ConstBonusEvaluator(int priority, bool isMultiply, IStat stat, decimal value)
            : this(priority, isMultiply, new Dictionary<IStat, decimal> { { stat, value } })
        {
        }
    }


    public record DynamicBonusEvaluator(int Priority, Func<IReadOnlyDictionary<IStat, decimal>, IEnumerable<ConstBonusEvaluator>> Func)
        : BonusEvaluator(Priority)
    {
        public DynamicBonusEvaluator(int priority, Func<IReadOnlyDictionary<IStat, decimal>, ConstBonusEvaluator> func)
            : this(priority, stateStats => [func(stateStats)])
        {
        }
    }
}