using Microsoft.Extensions.DependencyInjection;

using RqCalc.Model;

namespace RqCalc.Application;

public class CharacterCalculator(
    IValidator<ICharacterSource> validator,
    [FromKeyedServices(CharacterRawCalculator.Key)] ICharacterCalculator rawCalculator)
    : ICharacterCalculator
{
    public CharacterCalculationResult Calculate(ICharacterSource character)
    {
        validator.Validate(character);

        return rawCalculator.Calculate(character);
    }
}