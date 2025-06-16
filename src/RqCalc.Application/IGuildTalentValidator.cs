using RqCalc.Model;

namespace RqCalc.Application;

public interface IGuildTalentValidator
{
    void Validate(IGuildTalentBuildSource guildTalentBuildSource);
}