using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using RqCalc.Application;

namespace RqCalc.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRqCalc(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        return serviceCollection.AddSingleton<ICharacterCalculator, CharacterCalculator>();
    }
}