using RqCalc.Domain.GuildTalent;

namespace RqCalc.Model;

public interface IGuildTalentBuildSource
{
    IReadOnlyDictionary<IGuildTalent, int> GuildTalents { get; }
}