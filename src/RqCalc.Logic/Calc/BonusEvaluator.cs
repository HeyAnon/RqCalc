using RqCalc.Domain.Persistent;

namespace RqCalc.Logic.Calc;

public abstract class BonusEvaluator
{
    public readonly int Priority;


    protected BonusEvaluator(int priority)
    {
        this.Priority = priority;
    }


    public class ConstBonusEvaluator : BonusEvaluator
    {
        public readonly IReadOnlyDictionary<IStat, decimal> Stats;

        public readonly bool IsMultiply;


        public ConstBonusEvaluator(int priority, bool isMultiply, IReadOnlyDictionary<IStat, decimal> stats)
            : base(priority)
        {
            if (stats == null) throw new ArgumentNullException(nameof(stats));

            this.Stats = stats;
            this.IsMultiply = isMultiply;
        }

        public ConstBonusEvaluator(int priority, bool isMultiply, IStat stat, decimal value)
            : this(priority, isMultiply, new Dictionary<IStat, decimal> { { stat, value } })
        {

        }
    }


    public class DynamicBonusEvaluator : BonusEvaluator
    {
        public readonly Func<IReadOnlyDictionary<IStat, decimal>, IEnumerable<ConstBonusEvaluator>> Func;


        public DynamicBonusEvaluator(int priority, Func<IReadOnlyDictionary<IStat, decimal>, ConstBonusEvaluator> func)
            : this(priority, stateStats => new[] { func(stateStats) })
        {
        }

        public DynamicBonusEvaluator(int priority, Func<IReadOnlyDictionary<IStat, decimal>, IEnumerable<ConstBonusEvaluator>> func)
            : base(priority)
        {
            this.Func = func ?? throw new ArgumentNullException(nameof(func));
        }
    }
}