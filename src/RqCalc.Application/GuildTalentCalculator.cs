using Microsoft.Extensions.DependencyInjection;
using RqCalc.Model;

namespace RqCalc.Application;

public class GuildTalentCalculator(
    IValidator<IGuildTalentBuildSource> validator,
    [FromKeyedServices(GuildTalentRawCalculator.Key)] IGuildTalentCalculator rawCalculator) : IGuildTalentCalculator
{
    public int GetFreeGuildTalents(IGuildTalentBuildSource source)
    {
        validator.Validate(source);

        return rawCalculator.GetFreeGuildTalents(source);
    }
}