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

public interface IModelFactory
{
    EquipmentChangeModel CreateEquipmentModel(CharacterEquipmentIdentity identity);

    EquipmentDataChangeModel CreateEquipmentDataModel(ICharacterEquipmentData characterEquipmentData);

    ActiveImageChangeModel<IElixir> CreateElixirModel();

    ActiveImageChangeModelCollection<IImageDirectoryBase, IConsumable> CreateConsumableListModel();

    CharacterStatEditModel CreateCharacterStatEditModel(CharacterChangeModel rootModel, IStat stat);

    ActiveImageChangeModelDictionary<IImageDirectoryBase, IGuildTalent, int> CreateGuildTalentDict();

    ActiveImageChangeModelCollection<IImageDirectoryBase, ITalent> CreateTalentListModel();

    ActiveImageChangeModel<IAura> CreateAuraModel();

    ActiveImageChangeModelDictionary<IImageDirectoryBase, IAura, bool> CreateSharedAurasDictModel();

    ActiveImageChangeModelDictionary<IImageDirectoryBase, IBuff, int> CreateBuffDictModel();

    ActiveImageChangeModelCollection<IImageDirectoryBase, ICollectedItem> CreateCollectedEquipmentListModel();

    CharacterChangeModel CreateCharacterChangeModel(string? code);


    MainWindowModel CreateMainWindowModel();

    AboutWindowModel CreateAboutWindowModel();

    ElixirWindowModel CreateElixirWindowModel(ICharacterSource character);

    EquipmentWindowModel CreateEquipmentWindowModel(
        IReadOnlyDictionary<TextTemplateVariableType, decimal> evaluateStat,
        ICharacterSourceBase character,
        IEquipmentSlot slot,
        ICharacterEquipmentData? currentData,
        IEquipment? reverseEquipment);

    ConsumablesWindowModel CreateConsumablesWindowModel(IReadOnlyList<IConsumable> consumables);

    GuildTalentsWindowModel CreateGuildTalentsWindowModel(IGuildTalentBuildSource talentBuildSource);

    TalentsWindowModel CreateTalentsWindowModel(
        ITalentBuildSource talentBuildSource,
        IReadOnlyDictionary<TextTemplateVariableType, decimal> evaluateStats);

    AurasWindowModel CreateAurasWindowModel(IClass currentClass, IAura? currentAura, int currentLevel, IReadOnlyDictionary<IAura, bool> startupSharedAuras);

    BuffsWindowModel CreateBuffsWindowModel(ICharacterSource character);

    CollectionsWindowModel CreateCollectionsWindowModel(ICharacterSource character);

    CardWindowModel CreateCardWindowModel(
        IReadOnlyDictionary<TextTemplateVariableType, decimal> evaluateStats,
        IEquipmentType equipmentType,
        int cardIndex,
        IClass currentClass,
        ICard? startupCard,
        IEquipmentClass equipmentClass);

    StampWindowModel CreateStampWindowModel(IEquipment equipment, IClass currentClass, IStampVariant? startupStampVariant);
}
