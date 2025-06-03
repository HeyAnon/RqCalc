using RqCalc.Domain.Persistent._Base._Blocks;
using RqCalc.Domain.Persistent.Talent;

namespace RqCalc.Domain.Model;

public interface ITalentBuildSource : IClassObject, ILevelObject
{
    IReadOnlyList<ITalent> Talents { get; }
}

public static class TalentBuildSourceExtensions
{
    public static ITalentBuildSource OverrideTalents(this ITalentBuildSource source, IEnumerable<ITalent> talents)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (talents == null) throw new ArgumentNullException(nameof(talents));

        return new TalentBuildSource
        {
            Class = source.Class,
                
            Level = source.Level,
                
            Talents = talents.ToList()
        };
    }
}