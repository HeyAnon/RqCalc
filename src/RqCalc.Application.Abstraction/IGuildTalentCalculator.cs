using RqCalc.Model;

namespace RqCalc.Application;

public interface IGuildTalentCalculator
{
    int GetFreeGuildTalents(IGuildTalentBuildSource guildTalentBuildSource);
}