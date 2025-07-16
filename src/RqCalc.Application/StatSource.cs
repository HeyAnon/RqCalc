using Framework.Core;
using Framework.DataBase;
using RqCalc.Application.Calculation;
using RqCalc.Domain;
using RqCalc.Domain._Base;
using RqCalc.Domain._Extensions;

namespace RqCalc.Application;

public class StatService : IStatService
{
    public StatService(IDataSource<IPersistentDomainObjectBase> dataSource)
    {
        var stats = dataSource.GetFullList<IStat>();

        this.DependencyStatLayers = this.GetDependencyStatLayers(stats);
        this.StatPriority = this.DependencyStatLayers
            .SelectMany((layer, layerIndex) => layer.Select(stat => new { Stat = stat, Priority = layerIndex * BonusEvaluateRule.LayerStep }))
            .ToDictionary(pair => pair.Stat, pair => pair.Priority);

        this.EditStats = stats.Where(s => s.IsEditable).ToReadOnlyCollection();

        var classStats = dataSource.GetFullList<IClass>().SelectMany(@class => @class.GetStats());

        this.BaseStats = stats.Except(classStats.Distinct()).ToReadOnlyCollection();

        this.NotPrimaryEditStats = this.BaseStats.Where(s => s.IsEditable).ToReadOnlyCollection();

        this.SourcesStats = stats.Where(s => s.Sources.Any()).ToArray();
    }

    public IReadOnlyList<IStat> BaseStats { get; }

    public IReadOnlyDictionary<IStat, int> StatPriority { get; }

    public IReadOnlyList<IReadOnlyList<IStat>> DependencyStatLayers { get; }

    public IReadOnlyList<IStat> EditStats { get; }

    public IReadOnlyList<IStat> NotPrimaryEditStats { get; }

    public IReadOnlyList<IStat> SourcesStats { get; }

    public IEnumerable<IStat> GetEditStats(IClass @class) => new[] { @class.PrimaryStat }.Concat(this.NotPrimaryEditStats);

    private IReadOnlyList<IReadOnlyList<IStat>> GetDependencyStatLayers(IReadOnlyCollection<IStat> stats)
    {
        if (stats == null) throw new ArgumentNullException(nameof(stats));


        var request = from stat in stats

            let dependencyStats = stat.Bonuses.SelectMany(statBonus => statBonus.Type.Stats.ToList(bonusTypeStat => bonusTypeStat.Stat)).ToArray()

            where dependencyStats.AnyA()

            select new
            {
                Stat = stat,

                DependencyStats = dependencyStats
            };


        var depStats = request.ToList();


        var layers = new { Prev = Enumerable.Empty<IReadOnlyList<IStat>>(), Current = stats.ToList() }.Iterate(pair => pair.Current.Any(), state =>
        {
            var currentStats = state.Current;

            var currentDepStat = depStats.Where(pair => currentStats.Contains(pair.Stat)).ToList();

            return currentStats.Partial(stat => stat.Sources.All(source => source.Variables.All(v => v.TypeStat == null || state.Prev.Any(layer => layer.Contains(v.TypeStat))))

                                                && !currentDepStat.Any(statPair => statPair.DependencyStats.Contains(stat)), (prev, current) => new { Prev = state.Prev.Concat(
                [prev]), Current = current });
        });


        var result = layers.Prev.ToArray();

        return result;
    }
}
