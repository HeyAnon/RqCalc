using RqCalc.Domain.Talent;

namespace RqCalc.Model;

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