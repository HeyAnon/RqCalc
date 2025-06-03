using RqCalc.Domain.Persistent.GuildTalent;

namespace RqCalc.Domain.Model;

public interface IGuildTalentBuildSource
{
    IReadOnlyDictionary<IGuildTalent, int> GuildTalents { get; }
}