using Framework.DataBase;
using Framework.DependencyInjection;
using Framework.ExpressionParsers;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using RqCalc.Application;
using RqCalc.Application.Settings;
using RqCalc.DataBase.EntityFramework._DBContext;
using RqCalc.Domain._Base;

namespace RqCalc.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRqCalc(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        return serviceCollection

            .AddSingleton<IApplicationSettingsFactory, ApplicationSettingsFactory>()
            .AddSingletonFrom((IApplicationSettingsFactory factory) => factory.Create())

            .AddSingleton<ILastVersionService, LastVersionService>()
            .AddSingletonFrom((ILastVersionService lastVersionService) => lastVersionService.LastVersion)

            .AddSingleton<ICharacterCalculator, CharacterCalculator>()
            .AddSingleton<IStatService, StatService>()
            .AddSingleton<IClassService, ClassService>()
            .AddSingleton<IFormulaService, FormulaService>()
            .AddSingleton<IBonusTypeService, BonusTypeService>()
            .AddSingleton<IEquipmentForgeService, EquipmentForgeService>()

            .AddSingleton<ICharacterValidator, CharacterValidator>()
            .AddSingleton<IFreeStatCalculator, FreeStatCalculator>()
            .AddSingleton<ITalentValidator, TalentValidator>()
            .AddSingleton<IGuildTalentValidator, GuildTalentValidator>()

            .AddSingleton<IEquipmentSlotService, EquipmentSlotService>()

            .AddSingleton<INativeBodyExpressionParser, RoslynCSharpExpressionParser>()

            .AddKeyedSingleton(ImplementTypeResolver.Key, (_, _) => ImplementTypeResolver.Default)

            .AddDbContext<RqCalcDbContext>(options =>
                options.UseSqlite(configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Singleton, ServiceLifetime.Singleton)

            .AddSingletonFrom<IDataSource<IPersistentDomainObjectBase>, RqCalcDbContext>();
    }
}