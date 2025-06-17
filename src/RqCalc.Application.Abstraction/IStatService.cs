using RqCalc.Domain;

namespace RqCalc.Application;

public interface IStatService
{
    IReadOnlyList<IStat> BaseStats { get; }

    IReadOnlyDictionary<IStat, int> StatPriority { get; }

    IReadOnlyList<IReadOnlyList<IStat>> DependencyStatLayers { get; }
    
    IReadOnlyList<IStat> EditStats { get; }

    IReadOnlyList<IStat> NotPrimaryEditStats { get; }
    
    IReadOnlyList<IStat> SourcesStats { get; }

    IEnumerable<IStat> GetEditStats(IClass @class);
}