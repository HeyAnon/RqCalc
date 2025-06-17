using RqCalc.Model;

namespace RqCalc.Application;

public interface IGuildTalentCalculator
{
    //ISerializer<string, IGuildTalentBuildSource> GuildTalentSerializer { get; }

    int GetFreeGuildTalents(IGuildTalentBuildSource guildTalentBuildSource);
}