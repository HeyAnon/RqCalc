using Bss.Testing.Xunit.Interfaces;

using Framework.DependencyInjection;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using RqCalc.Infrastructure;

namespace RqCalc.Tests.__Support;

public class EnvironmentInitializer : IFrameworkInitializer
{
    public static readonly IConfiguration RootConfiguration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", false)
        .Build();

    public IServiceCollection ConfigureFramework(IServiceCollection services) =>
        services.AddSingleton(RootConfiguration)
                .ValidateDuplicateDeclaration();

    public IServiceProvider ConfigureTestEnvironment(IServiceCollection services, IConfiguration configuration) =>
        services
            .AddSingleton(configuration)
            .ValidateDuplicateDeclaration()
            .BuildServiceProvider(
                new ServiceProviderOptions
                {
                    ValidateScopes = true,
                    ValidateOnBuild = true
                });

    public IServiceProvider GetFrameworkServiceProvider()
    {
        var frameworkServices = new ServiceCollection();

        this.ConfigureFramework(frameworkServices);

        frameworkServices.TryAddSingleton<ITestServiceProviderPool, TestServiceProviderPool>();

        return this.ConfigureTestEnvironment(frameworkServices, RootConfiguration);
    }
}

public class TestServiceProviderPool : ITestServiceProviderPool
{
    public IServiceProvider Get() =>
        new ServiceCollection()
            .AddRqCalc(EnvironmentInitializer.RootConfiguration)
            .ValidateDuplicateDeclaration()
            .BuildServiceProvider(
                new ServiceProviderOptions
                {
                    ValidateScopes = true,
                    ValidateOnBuild = true
                });

    public void Release(IServiceProvider? serviceProvider)
    {

    }
}
