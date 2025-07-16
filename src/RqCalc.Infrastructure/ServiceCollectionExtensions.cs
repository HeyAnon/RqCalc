using Framework.Core;
using Framework.Core.Serialization;
using Framework.DataBase;
using Framework.DependencyInjection;
using Framework.ExpressionParsers;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using RqCalc.Application;
using RqCalc.Application.ImageSource;
using RqCalc.Application.Serializer.Character;
using RqCalc.Application.Serializer.GuildTalent;
using RqCalc.Application.Serializer.Talent;
using RqCalc.Application.Settings;
using RqCalc.DataBase.EntityFramework._DBContext;
using RqCalc.Domain._Base;
using RqCalc.Model;

namespace RqCalc.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRqCalc(this IServiceCollection serviceCollection, IConfiguration configuration) =>
        serviceCollection

            .AddSingleton<IApplicationSettingsFactory, ApplicationSettingsFactory>()
            .AddSingletonFrom((IApplicationSettingsFactory factory) => factory.Create())

            .AddSingleton<ILastVersionService, LastVersionService>()
            .AddSingletonFrom((ILastVersionService lastVersionService) => lastVersionService.LastVersion)

            .AddSingleton<IDefaultCharacterSource, DefaultCharacterSource>()

            .AddSingleton<IValidator<ICharacterSource>, CharacterValidator>()
            .AddSingleton<ICharacterCalculator, CharacterCalculator>()
            .AddKeyedSingleton<ICharacterCalculator, CharacterRawCalculator>(CharacterRawCalculator.Key)

            .AddSingleton<IValidator<ITalentBuildSource>, TalentValidator>()
            .AddSingleton<ITalentCalculator, TalentCalculator>()
            .AddKeyedSingleton<ITalentCalculator, TalentRawCalculator>(TalentRawCalculator.Key)

            .AddSingleton<IValidator<IGuildTalentBuildSource>, GuildTalentValidator>()
            .AddSingleton<IGuildTalentCalculator, GuildTalentCalculator>()
            .AddKeyedSingleton<IGuildTalentCalculator, GuildTalentRawCalculator>(GuildTalentRawCalculator.Key)


            .AddSingleton<IFreeStatCalculator, FreeStatCalculator>()
            .AddSingleton<IFreeTalentCalculator, FreeTalentCalculator>()

            .AddSingleton<IStatService, StatService>()
            .AddSingleton<IClassService, ClassService>()
            .AddSingleton<IFormulaService, FormulaService>()
            .AddSingleton<IBonusTypeService, BonusTypeService>()
            .AddSingleton<IEquipmentService, EquipmentService>()
            .AddSingleton<IEquipmentForgeService, EquipmentForgeService>()
            .AddSingleton<IEquipmentSlotService, EquipmentSlotService>()
            .AddSingleton<IStampService, StampService>()
            .AddSingleton<IAuraService, AuraService>()
            .AddSingleton<IAuraServiceFactory, AuraServiceFactory>()
    
            .AddSingleton<ISerializer<byte[], ICharacterSource>, CharacterSerializer>()
            .AddSingletonFrom((ISerializer<byte[], ICharacterSource> serializer) => serializer.Select(Serializer.Base64))
            .AddSingleton<ISerializer<byte[], ITalentBuildSource>, TalentBuildSerializer>()
            .AddSingletonFrom((ISerializer<byte[], ITalentBuildSource> serializer) => serializer.Select(Serializer.Base64))
            .AddSingleton<ISerializer<byte[], IGuildTalentBuildSource>, GuildTalentBuildSerializer>()
            .AddSingletonFrom((ISerializer<byte[], IGuildTalentBuildSource> serializer) => serializer.Select(Serializer.Base64))

            .AddSingleton(TypeResolverHelper.Create(TypeSource.FromSample<IPersistentDomainObjectBase>(), TypeSearchMode.Name))
            .AddSingleton<ImageSourceFuncCache>()
            .AddSingleton<IImageSourceService, ImageSourceService>()

            .AddSingleton<INativeExpressionParser>(CSharpNativeExpressionParser.Compile)

            .AddKeyedSingleton(ImplementTypeResolver.Key, (_, _) => ImplementTypeResolver.Default)

            .AddDbContext<RqCalcDbContext>(
                options =>
                    options.UseSqlite(configuration.GetConnectionString("DefaultConnection")),
                ServiceLifetime.Singleton,
                ServiceLifetime.Singleton)

            .AddSingletonFrom<IDataSource<IPersistentDomainObjectBase>, RqCalcDbContext>();
}
