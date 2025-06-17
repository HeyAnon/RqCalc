using Bss.Testing.Xunit.Interfaces;

using Framework.DependencyInjection;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using RqCalc.Infrastructure;


namespace RqCalc.Tests.__Support;

public class EnvironmentInitializer : IFrameworkInitializer
{
    public IServiceCollection ConfigureFramework(IServiceCollection services)
    {
        return services;
    }

    public IServiceProvider ConfigureTestEnvironment(IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddRqCalc(configuration)
            .ValidateDuplicateDeclaration()
            .BuildServiceProvider(
                new ServiceProviderOptions
                {
                    ValidateScopes = true,
                    ValidateOnBuild = true
                });
    }

    public IServiceProvider GetFrameworkServiceProvider()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false)
            .Build();

        return this.ConfigureTestEnvironment(new ServiceCollection(), configuration);
    }
}