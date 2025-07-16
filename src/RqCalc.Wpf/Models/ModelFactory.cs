using Framework.Core.Serialization;
using Framework.DataBase;

using RqCalc.Application;
using RqCalc.Application.ImageSource;
using RqCalc.Domain;
using RqCalc.Domain._Base;
using RqCalc.Domain.Card;
using RqCalc.Domain.CollectedStatistic;
using RqCalc.Domain.Equipment;
using RqCalc.Domain.GuildTalent;
using RqCalc.Domain.Stamp;
using RqCalc.Domain.Talent;
using RqCalc.Model;
using RqCalc.Wpf.Models._Base;
using RqCalc.Wpf.Models.Window;
using RqCalc.Wpf.Models.Window.Dialog;

namespace RqCalc.Wpf.Models;

public class ModelFactory(
    ApplicationSettings settings,
    IDataSource<IPersistentDomainObjectBase> dataSource,
    IVersion lastVersion,
    WpfApplicationSettings wpfApplicationSettings,
    IImageSourceService imageSourceService,
    IDefaultCharacterSource defaultCharacterSource,
    ISerializer<string, ICharacterSource> characterSerializer,
    ISerializer<byte[], ICharacterSource> binaryCharacterSerializer,
    IFreeStatCalculator freeStatCalculator,
    IStatService statService,
    IEquipmentService equipmentService,
    IEquipmentSlotService equipmentSlotService,
    IStampService stampService,
    ITalentCalculator talentCalculator,
    ICharacterCalculator characterCalculator,
    ISerializer<string, IGuildTalentBuildSource> guildTalentSerializer,
    IGuildTalentCalculator guildTalentCalculator,
    IFreeTalentCalculator freeTalentCalculator,
    ISerializer<string, ITalentBuildSource> talentSerializer,
    IAuraService auraService) : IModelFactory
{
    public EquipmentChangeModel CreateEquipmentModel(CharacterEquipmentIdentity identity) => new(this, identity);

    public EquipmentDataChangeModel CreateEquipmentDataModel(ICharacterEquipmentData characterEquipmentData) =>
        new(imageSourceService, equipmentService, characterEquipmentData);

    public ActiveImageChangeModel<IElixir> CreateElixirModel() =>
        new(imageSourceService.GetStaticImage(StaticImageType.Elixir)) { Activate = true };

    public ActiveImageChangeModelCollection<IImageDirectoryBase, IConsumable> CreateConsumableListModel() =>
        new(imageSourceService.GetStaticImage(StaticImageType.Consumable)) { Activate = true };

    public CharacterStatEditModel CreateCharacterStatEditModel(CharacterChangeModel rootModel, IStat stat) =>
        new(settings, rootModel) { Stat = stat };

    public ActiveImageChangeModelDictionary<IImageDirectoryBase, IGuildTalent, int> CreateGuildTalentDict() =>
        new(imageSourceService.GetStaticImage(StaticImageType.Guild)) { Activate = true };

    public ActiveImageChangeModelCollection<IImageDirectoryBase, ITalent> CreateTalentListModel() =>
        new(imageSourceService.GetStaticImage(StaticImageType.Talent)) { Activate = true };

    public ActiveImageChangeModel<IAura> CreateAuraModel() =>
        new(imageSourceService.GetStaticImage(StaticImageType.Aura)) { Activate = true };

    public ActiveImageChangeModelDictionary<IImageDirectoryBase, IAura, bool> CreateSharedAurasDictModel() => new();

    public ActiveImageChangeModelDictionary<IImageDirectoryBase, IBuff, int> CreateBuffDictModel() =>
        new(imageSourceService.GetStaticImage(StaticImageType.Buff)) { Activate = true };

    public ActiveImageChangeModelCollection<IImageDirectoryBase, ICollectedItem> CreateCollectedEquipmentListModel() =>
        new(imageSourceService.GetStaticImage(StaticImageType.Collections)) { Activate = true };

    public MainWindowModel CreateMainWindowModel() => new(this, wpfApplicationSettings, settings, dataSource, lastVersion);

    public AboutWindowModel CreateAboutWindowModel() => new(wpfApplicationSettings, lastVersion);

    public ElixirWindowModel CreateElixirWindowModel(ICharacterSource character) => new(dataSource, character.Class, character.Elixir);

    public CharacterChangeModel CreateCharacterChangeModel(string? code)
    {
        var character = code == null ? defaultCharacterSource.GetDefaultCharacter() : characterSerializer.Deserialize(code);

        return new CharacterChangeModel(
            this,
            settings,
            lastVersion,
            dataSource,
            characterSerializer,
            binaryCharacterSerializer,
            freeStatCalculator,
            freeTalentCalculator,
            talentCalculator,
            characterCalculator,
            statService,
            stampService,
            character);
    }


    public EquipmentWindowModel CreateEquipmentWindowModel(
        IReadOnlyDictionary<TextTemplateVariableType, decimal> evaluateStat,
        ICharacterSourceBase character,
        IEquipmentSlot slot,
        ICharacterEquipmentData? currentData,
        IEquipment? reverseEquipment) =>
        new(
            this,
            lastVersion,
            dataSource,
            equipmentSlotService,
            imageSourceService,
            stampService,
            equipmentService,
            evaluateStat,
            character,
            slot,
            currentData,
            reverseEquipment);

    public ConsumablesWindowModel CreateConsumablesWindowModel(IReadOnlyList<IConsumable> consumables) =>
        new(dataSource) { Consumables = consumables };

    public GuildTalentsWindowModel CreateGuildTalentsWindowModel(IGuildTalentBuildSource talentBuildSource) => new(
        guildTalentSerializer,
        guildTalentCalculator,
        dataSource,
        talentBuildSource);

    public TalentsWindowModel CreateTalentsWindowModel(
        ITalentBuildSource talentBuildSource,
        IReadOnlyDictionary<TextTemplateVariableType, decimal> evaluateStats) => new(
        talentSerializer,
        freeTalentCalculator,
        talentCalculator,
        talentBuildSource,
        evaluateStats);

    public AurasWindowModel CreateAurasWindowModel(
        IClass currentClass,
        IAura? currentAura,
        int currentLevel,
        IReadOnlyDictionary<IAura, bool> startupSharedAuras) => new(
        lastVersion,
        auraService,
        currentClass,
        currentAura,
        currentLevel,
        startupSharedAuras);

    public BuffsWindowModel CreateBuffsWindowModel(ICharacterSource character) => new(dataSource, lastVersion, character);

    public CollectionsWindowModel CreateCollectionsWindowModel(ICharacterSource character) => new (lastVersion, dataSource, character);

    public CardWindowModel CreateCardWindowModel(
        IReadOnlyDictionary<TextTemplateVariableType, decimal> evaluateStats,
        IEquipmentType equipmentType,
        int cardIndex,
        IClass currentClass,
        ICard? startupCard,
        IEquipmentClass equipmentClass) => new(
        lastVersion,
        dataSource,
        evaluateStats,
        equipmentType,
        cardIndex,
        currentClass,
        startupCard,
        equipmentClass);

    public StampWindowModel CreateStampWindowModel(IEquipment equipment, IClass currentClass, IStampVariant? startupStampVariant) =>
        new(dataSource, stampService, equipment, currentClass, startupStampVariant);
}