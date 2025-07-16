using Framework.DataBase;

using RqCalc.Application.Calculation;
using RqCalc.Domain._Base;
using RqCalc.Domain.BonusType;
using RqCalc.Model;

namespace RqCalc.Application;

public class BonusTypeService : IBonusTypeService
{
    private readonly IDataSource<IPersistentDomainObjectBase> dataSource;

    private readonly IStatService statService;

    public BonusTypeService(
        ApplicationSettings settings,
        IDataSource<IPersistentDomainObjectBase> dataSource,
        IStatService statService)
    {
        this.dataSource = dataSource;
        this.statService = statService;

        var dependencyBonusTypeLayers = this.GetDependencyBonusTypeLayers();

        this.BonusTypePriority = dependencyBonusTypeLayers.SelectMany((layer, layerIndex) => layer.Select(bonus => new
                                                                          {
                                                                              Bonus = bonus, Priority = layerIndex * BonusEvaluateRule.LayerStep + (bonus.IsMultiply!.Value ? BonusEvaluateRule.MultiplyOffset : BonusEvaluateRule.SumOffset)
                                                                          }))
                                                          .ToDictionary(pair => pair.Bonus, pair => pair.Priority);

        this.AttackBonusTypes = dataSource.GetFullList<IBonusType>().Where(bonusType => bonusType.Stats.Any() && bonusType.Stats.All(bts => bts.Stat == settings.AttackStat))
                                          .ToList();
    }

    public IReadOnlyList<IBonusType> AttackBonusTypes { get; }

    public IReadOnlyDictionary<IBonusType, int> BonusTypePriority { get; }

    private IReadOnlyList<IReadOnlyList<IBonusType>> GetDependencyBonusTypeLayers()
    {
        var groupRequest =

            from bonusType in this.dataSource.GetFullList<IBonusType>()

            group bonusType by this.statService.DependencyStatLayers.FirstOrDefault(statLayer => bonusType.Stats.Any(bts => statLayer.Contains(bts.Stat)));

        var joinRequest =

            from statLayer in this.statService.DependencyStatLayers

            join g in groupRequest on statLayer equals g.Key

            select g.ToArray();

        return joinRequest.ToArray();
    }
}