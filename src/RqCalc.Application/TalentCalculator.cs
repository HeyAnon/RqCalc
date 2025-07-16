using Microsoft.Extensions.DependencyInjection;

using RqCalc.Domain.Talent;
using RqCalc.Model;

namespace RqCalc.Application;

public class TalentCalculator(
    IValidator<ITalentBuildSource> validator,
    [FromKeyedServices(TalentRawCalculator.Key)] ITalentCalculator rawCalculator) : ITalentCalculator
{
    public IEnumerable<ITalent> GetLimitedTalents(ITalentBuildSource source)
    {
        validator.Validate(source);

        return rawCalculator.GetLimitedTalents(source);
    }
}