using Framework.Core;
using Framework.Core.Serialization;
using Framework.DataBase;
using Framework.HierarchicalExpand;
using Framework.Reactive;
using Framework.Reactive.ObservableRecurse;

using RqCalc.Application;
using RqCalc.Domain;
using RqCalc.Domain._Base;
using RqCalc.Domain._Extensions;
using RqCalc.Domain.CollectedStatistic;
using RqCalc.Domain.Equipment;
using RqCalc.Domain.GuildTalent;
using RqCalc.Domain.Talent;
using RqCalc.Model;
using RqCalc.Model._Extensions;
using RqCalc.Model.Impl;
using RqCalc.Wpf._Extensions;
using RqCalc.Wpf.Exception;
using RqCalc.Wpf.Models._Base;
using RqCalc.Wpf.Models.Window.Dialog;

namespace RqCalc.Wpf.Models;

public class CharacterChangeModel : UpdateModel, ICharacterSource
{
    private readonly IModelFactory modelFactory;

    private readonly ApplicationSettings settings;

    private readonly IVersion lastVersion;

    private readonly IDataSource<IPersistentDomainObjectBase> dataSource;

    private readonly ISerializer<string, ICharacterSource> characterSerializer;

    private readonly ISerializer<byte[], ICharacterSource> binaryCharacterSerializer;

    private readonly IFreeStatCalculator freeStatCalculator;

    private readonly IFreeTalentCalculator freeTalentCalculator;

    private readonly ITalentCalculator talentCalculator;

    private readonly ICharacterCalculator characterCalculator;

    private readonly IStampService stampService;

    public CharacterChangeModel(
        IModelFactory modelFactory,
        ApplicationSettings settings,
        IVersion lastVersion,
        IDataSource<IPersistentDomainObjectBase> dataSource,
        ISerializer<string, ICharacterSource> characterSerializer,
        ISerializer<byte[], ICharacterSource> binaryCharacterSerializer,
        IFreeStatCalculator freeStatCalculator,
        IFreeTalentCalculator freeTalentCalculator,
        ITalentCalculator talentCalculator,
        ICharacterCalculator characterCalculator,
        IStatService statService,
        IStampService stampService,
        ICharacterSource character)

    {
        this.modelFactory = modelFactory;
        this.settings = settings;
        this.lastVersion = lastVersion;
        this.dataSource = dataSource;
        this.characterSerializer = characterSerializer;
        this.binaryCharacterSerializer = binaryCharacterSerializer;
        this.freeStatCalculator = freeStatCalculator;
        this.freeTalentCalculator = freeTalentCalculator;
        this.talentCalculator = talentCalculator;
        this.characterCalculator = characterCalculator;
        this.stampService = stampService;
        {
            var equipmentsRequest =

                from slot in dataSource.GetFullList<IEquipmentSlot>()

                from index in Enumerable.Range(0, slot.Count)

                select modelFactory.CreateEquipmentModel(new CharacterEquipmentIdentity(slot, index));

            this.Equipments = equipmentsRequest.ToObservableCollection();

            {
                var dict = this.Equipments.ToDictionary(model => model.Identity);

                foreach (var equipmentModel in this.Equipments)
                {
                    equipmentModel.ReverseModel = equipmentModel.Identity.GetReverse().Maybe(ri => dict[ri]);
                }
            }

            this.EquipmentDict =
                this.Equipments.ToDictionary(model => model.Identity.Slot.Id
                                                      + (model.Identity.Index == 0 ? "" : "|" + model.Identity.Index));
        }


        {
            var classDisplayStats = StatConst.SpecialStats.ToDictionary(v => v, _ => new DisplayStatModel());

            var staticDisplayStats = dataSource
                                     .GetFullList<IStat>()
                                     .Except(dataSource.GetFullList<IClass>().SelectMany(c => c.GetStats()))
                                     .ToDictionary(stat => stat.GetBindName(), stat => new DisplayStatModel { Stat = stat });

            this.DisplayStats = classDisplayStats.Concat(staticDisplayStats);
        }

        {
            this.EditStats =
                new Dictionary<string, CharacterStatEditModel>
                {
                    { StatConst.PrimaryStatName, modelFactory.CreateCharacterStatEditModel(this, null!) }
                }.Concat(
                    statService.NotPrimaryEditStats.ToDictionary(
                        stat => stat.Name,
                        stat => modelFactory.CreateCharacterStatEditModel(this, stat)));

            this.EditStatList = this.EditStats.Values.ToObservableCollection();
        }

        this.ElixirModel = modelFactory.CreateElixirModel();

        this.ConsumableList = modelFactory.CreateConsumableListModel();

        this.GuildTalentDict = modelFactory.CreateGuildTalentDict();

        this.TalentList = modelFactory.CreateTalentListModel();

        this.AuraModel = modelFactory.CreateAuraModel();

        this.SharedAurasDict = modelFactory.CreateSharedAurasDictModel();

        this.BuffDict = modelFactory.CreateBuffDictModel();

        this.CollectedEquipmentList = modelFactory.CreateCollectedEquipmentListModel();

        this.SetData(character);

        this.EditStats.Join(
                this.DisplayStats,
                pair => pair.Key,
                pair => pair.Key,
                (editModelPair, displayModelPair) => new { DisplayModel = displayModelPair.Value, EditModel = editModelPair.Value })
            .Foreach(pair =>

                         pair.DisplayModel.SubscribeExplicit(rule => rule.Subscribe(
                                                                 model => model.Value,
                                                                 value => pair.EditModel.DisplayValue = (int)value.Value)));

        this.SubscribeExplicit(

            rule => rule.Subscribe(model => model.EnableAura, this.Recalculate),
            rule => rule.Subscribe(model => model.EnableBuffs, this.Recalculate),
            rule => rule.Subscribe(model => model.EnableGuildTalents, this.Recalculate),
            rule => rule.Subscribe(model => model.EnableTalents, this.Recalculate),
            rule => rule.Subscribe(model => model.EnableCollecting, this.Recalculate),

            rule => rule.Subscribe(model => model.LostControl, this.Recalculate),

            rule => rule.SelectMany(
                model => model.EditStatList,
                editStatModel => editStatModel.Subscribe(model => model.EditValue, this.Recalculate)),

            rule => rule.Subscribe(model => model.Gender, this.GenderChanged),

            rule => rule.Subscribe(model => model.Class, this.ClassChanged),

            rule => rule.Subscribe(model => model.Level, this.LevelChanged),

            rule => rule.Subscribe(model => model.Event, this.Recalculate),

            rule => rule.Subscribe(model => model.State, this.Recalculate),

            rule => rule.SelectMany(
                model => model.Equipments,
                equipmentModelRule => equipmentModelRule.Subscribe(model => model.DataModel, this.DataModelChanged),
                equipmentModelRule => equipmentModelRule.Select(
                    model => model.DataModel,
                    dataModelRule => dataModelRule.Subscribe(dataModel => dataModel.Active, this.Recalculate))),

            rule => rule.Select(
                model => model.ElixirModel,
                extRule => extRule.Subscribe(obj => obj.Active, this.Recalculate),
                extRule => extRule.Subscribe(obj => obj.SelectedObject, this.Recalculate)),

            rule => rule.Select(
                model => model.ConsumableList,
                extRule => extRule.Subscribe(obj => obj.Active, this.Recalculate),
                extRule => extRule.Subscribe(obj => obj.Value, this.Recalculate)),

            rule => rule.Select(
                model => model.GuildTalentDict,
                extRule => extRule.Subscribe(obj => obj.Active, this.Recalculate),
                extRule => extRule.Subscribe(obj => obj.Value, this.Recalculate)),

            rule => rule.Select(
                model => model.TalentList,
                extRule => extRule.Subscribe(obj => obj.Active, this.TalentsChanged),
                extRule => extRule.Subscribe(obj => obj.Value, this.TalentsChanged)),

            rule => rule.Select(
                model => model.AuraModel,
                extRule => extRule.Subscribe(obj => obj.Active, this.Recalculate),
                extRule => extRule.Subscribe(obj => obj.SelectedObject, this.Recalculate)),

            rule => rule.Select(model => model.SharedAurasDict, extRule => extRule.Subscribe(obj => obj.Value, this.Recalculate)),

            rule => rule.Select(
                model => model.BuffDict,
                extRule => extRule.Subscribe(obj => obj.Active, this.Recalculate),
                extRule => extRule.Subscribe(obj => obj.Value, this.Recalculate)),

            rule => rule.Select(
                model => model.CollectedEquipmentList,
                extRule => extRule.Subscribe(obj => obj.Active, this.Recalculate),
                extRule => extRule.Subscribe(obj => obj.Value, this.Recalculate)));
    }


    public IReadOnlyCollection<IStat> GetStats() => this.dataSource.GetFullList<IStat>();

    public ElixirWindowModel CreateElixirWindowModel()
    {
        return this.modelFactory.CreateElixirWindowModel(this);
    }

    public ConsumablesWindowModel CreateConsumablesWindowModel()
    {
        return this.modelFactory.CreateConsumablesWindowModel(this.Consumables);
    }

    public GuildTalentsWindowModel CreateGuildTalentsWindowModel()
    {
        return this.modelFactory.CreateGuildTalentsWindowModel(this);
    }

    public TalentsWindowModel CreateTalentsWindowModel()
    {
        return this.modelFactory.CreateTalentsWindowModel(this, this.GetTemplateEvaluateStats());
    }

    public AurasWindowModel CreateAurasWindowModel()
    {
        return this.modelFactory.CreateAurasWindowModel(this.Class, this.Aura, this.Level, this.SharedAuras);
    }

    public BuffsWindowModel CreateBuffsWindowModel()
    {
        return this.modelFactory.CreateBuffsWindowModel(this);
    }

    public CollectionsWindowModel CreateCollectionsWindowModel()
    {
        return this.modelFactory.CreateCollectionsWindowModel(this);
    }


    public EquipmentWindowModel CreateEquipmentWindowModel(EquipmentChangeModel baseEquipmentModel)
    {
        var editModel = this.GetEquipmentEditModel(baseEquipmentModel);

        return this.modelFactory.CreateEquipmentWindowModel(
            this.GetTemplateEvaluateStats(),
            this,
            editModel.Identity.Slot,
            editModel.Data,
            editModel.ReverseModel.Maybe(rm => rm.Data).Maybe(data => data.Equipment));
    }

    public string Code
    {
        get => this.GetValue(v => v.Code);
        set
        {
            if (this.Code == value)
            {
                return;
            }

            var prevValue = this.Code;

            this.SetValue(v => v.Code, value);

            try
            {
                this.SetData(this.characterSerializer.Deserialize(this.Code));
            }
            catch (System.Exception ex)
            {
                this.SetValue(v => v.Code, prevValue);

                throw new ClientException("Invalid code", ex);
            }
        }
    }

    public byte[] BinaryData
    {
        get => this.binaryCharacterSerializer.Serialize(this);
        set => this.SetData(this.binaryCharacterSerializer.Deserialize(value));
    }


    private CharacterStatEditModel PrimaryEditStat => this.EditStats[StatConst.PrimaryStatName];


    private DisplayStatModel PrimaryDisplayStat => this.DisplayStats[StatConst.PrimaryStatName];

    private DisplayStatModel EnergyDisplayStat => this.DisplayStats[StatConst.EnergyStatName];

    private DisplayStatModel RestoreEnergyDisplayStat => this.DisplayStats[StatConst.RestoreEnergyStatName];

    private DisplayStatModel RestoreEnergyPerHitStat => this.DisplayStats[StatConst.RestoreEnergyPerHitStatName];

    private DisplayStatModel RestoreEnergyPerKillStat => this.DisplayStats[StatConst.RestoreEnergyPerKillStatName];


    public IReadOnlyDictionary<string, DisplayStatModel> DisplayStats
    {
        get => this.GetValue(v => v.DisplayStats);
        private set => this.SetValue(v => v.DisplayStats, value);
    }


    private ObservableCollection<CharacterStatEditModel> EditStatList
    {
        get => this.GetValue(v => v.EditStatList);
        set => this.SetValue(v => v.EditStatList, value);
    }

    public IReadOnlyDictionary<string, CharacterStatEditModel> EditStats
    {
        get => this.GetValue(v => v.EditStats);
        private set => this.SetValue(v => v.EditStats, value);
    }

    public IReadOnlyDictionary<string, EquipmentChangeModel> EquipmentDict
    {
        get => this.GetValue(v => v.EquipmentDict);
        private set => this.SetValue(v => v.EquipmentDict, value);
    }


    private ObservableCollection<EquipmentChangeModel> Equipments
    {
        get => this.GetValue(v => v.Equipments);
        set => this.SetValue(v => v.Equipments, value);
    }

    public int MinLevel { get => this.GetValue(v => v.MinLevel); private set => this.SetValue(v => v.MinLevel, value); }

    public int MaxLevel { get => this.GetValue(v => v.MaxLevel); private set => this.SetValue(v => v.MaxLevel, value); }

    public int Level { get => this.GetValue(v => v.Level); set => this.SetValue(v => v.Level, value); }


    public IStat PrimaryStat { get => this.GetValue(v => v.PrimaryStat); private set => this.SetValue(v => v.PrimaryStat, value); }

    public IStat EnergyStat { get => this.GetValue(v => v.EnergyStat); private set => this.SetValue(v => v.EnergyStat, value); }

    public IGender Gender { get => this.GetValue(v => v.Gender); set => this.SetValue(v => v.Gender, value); }

    public IClass Class { get => this.GetValue(v => v.Class); set => this.SetValue(v => v.Class, value); }

    public IState State { get => this.GetValue(v => v.State); set => this.SetValue(v => v.State, value); }

    public IEvent? Event { get => this.GetValue(v => v.Event); set => this.SetValue(v => v.Event, value); }


    public IImage CharacterImage
    {
        get => this.GetValue(v => v.CharacterImage);
        private set => this.SetValue(v => v.CharacterImage, value);
    }



    public IElixir? Elixir { get => this.ElixirModel.SelectedObject; set => this.ElixirModel.SelectedObject = value; }

    public ActiveImageChangeModel<IElixir> ElixirModel
    {
        get => this.GetValue(v => v.ElixirModel);
        set => this.SetValue(v => v.ElixirModel, value);
    }


    public IReadOnlyList<IConsumable> Consumables { get => this.ConsumableList.Value; set => this.ConsumableList.Value = value; }

    public ActiveImageChangeModelCollection<IImageDirectoryBase, IConsumable> ConsumableList
    {
        get => this.GetValue(v => v.ConsumableList);
        private set => this.SetValue(v => v.ConsumableList, value);
    }


    public IReadOnlyDictionary<IGuildTalent, int> GuildTalents
    {
        get => this.GuildTalentDict.Value;
        set => this.GuildTalentDict.Value = value.Where(buff => buff.Value != 0).ToDictionary();
    }

    public ActiveImageChangeModelDictionary<IImageDirectoryBase, IGuildTalent, int> GuildTalentDict
    {
        get => this.GetValue(v => v.GuildTalentDict);
        private set => this.SetValue(v => v.GuildTalentDict, value);
    }



    public IReadOnlyDictionary<IAura, bool> SharedAuras { get => this.SharedAurasDict.Value; set => this.SharedAurasDict.Value = value; }

    public ActiveImageChangeModelDictionary<IImageDirectoryBase, IAura, bool> SharedAurasDict
    {
        get => this.GetValue(v => v.SharedAurasDict);
        private set => this.SetValue(v => v.SharedAurasDict, value);
    }

    public IReadOnlyList<ITalent> Talents { get => this.TalentList.Value; set => this.TalentList.Value = value; }

    public ActiveImageChangeModelCollection<IImageDirectoryBase, ITalent> TalentList
    {
        get => this.GetValue(v => v.TalentList);
        private set => this.SetValue(v => v.TalentList, value);
    }


    public IAura? Aura { get => this.AuraModel.SelectedObject; set => this.AuraModel.SelectedObject = value; }

    public ActiveImageChangeModel<IAura> AuraModel
    {
        get => this.GetValue(v => v.AuraModel);
        set => this.SetValue(v => v.AuraModel, value);
    }

    public IReadOnlyDictionary<IBuff, int> Buffs
    {
        get => this.BuffDict.Value;
        set => this.BuffDict.Value = value.Where(buff => buff.Value != 0).ToDictionary();
    }

    public IReadOnlyList<ICollectedItem> CollectedItems
    {
        get => this.CollectedEquipmentList.Value;
        set => this.CollectedEquipmentList.Value = value;
    }

    public ActiveImageChangeModelCollection<IImageDirectoryBase, ICollectedItem> CollectedEquipmentList
    {
        get => this.GetValue(v => v.CollectedEquipmentList);
        private set => this.SetValue(v => v.CollectedEquipmentList, value);
    }

    public ActiveImageChangeModelDictionary<IImageDirectoryBase, IBuff, int> BuffDict
    {
        get => this.GetValue(v => v.BuffDict);
        private set => this.SetValue(v => v.BuffDict, value);
    }



    public bool EnableElixir { get => this.ElixirModel.Active; private set => this.ElixirModel.Active = value; }

    public bool EnableConsumables { get => this.ConsumableList.Active; private set => this.ConsumableList.Active = value; }

    public bool EnableGuildTalents { get => this.GuildTalentDict.Active; private set => this.GuildTalentDict.Active = value; }

    public bool EnableTalents { get => this.TalentList.Active; private set => this.TalentList.Active = value; }

    public bool LostControl { get => this.GetValue(v => v.LostControl); set => this.SetValue(v => v.LostControl, value); }

    public bool EnableCollecting { get => this.CollectedEquipmentList.Active; set => this.CollectedEquipmentList.Active = value; }

    public bool EnableAura { get => this.AuraModel.Active; private set => this.AuraModel.Active = value; }

    public bool EnableBuffs { get => this.BuffDict.Active; private set => this.BuffDict.Active = value; }



    public int FreeStats { get => this.GetValue(v => v.FreeStats); private set => this.SetValue(v => v.FreeStats, value); }



    public void UpdateAura(IAura? aura, IReadOnlyDictionary<IAura, bool> sharedAuras) =>
        this.UpdateOperation(() =>
                             {
                                 this.Aura = aura;
                                 this.SharedAuras = sharedAuras;
                             });

    public EquipmentChangeModel GetEquipmentEditModel(EquipmentChangeModel model)
    {
        if (model.Identity.Slot.IsExtraSlot())
        {
            if (model.ReverseModel!.IsDoubleHand)
            {
                return model.ReverseModel;
            }

            if (!model.ReverseModel.Identity.Slot.AllowSingleHand(this.Class))
            {
                return model.ReverseModel;
            }
        }

        return model;
    }


    private void SetData(ICharacterSource character)
    {
        if (character == null) throw new ArgumentNullException(nameof(character));

        this.UpdateOperation(
            () =>
            {
                this.Gender = character.Gender;
                this.Class = character.Class;
                this.Level = character.Level;
                this.State = character.State;
                this.Event = character.Event;

                this.Elixir = character.Elixir;
                this.Consumables = character.Consumables;
                this.GuildTalents = character.GuildTalents;
                this.Aura = character.Aura;
                this.Buffs = character.Buffs;
                this.Talents = character.Talents;
                this.CollectedItems = character.CollectedItems;

                this.SharedAuras = character.SharedAuras;

                this.EnableElixir = character.EnableElixir;
                this.EnableConsumables = character.EnableConsumables;
                this.EnableGuildTalents = character.EnableGuildTalents;
                this.EnableAura = character.EnableAura;
                this.EnableBuffs = character.EnableBuffs;
                this.EnableTalents = character.EnableTalents;
                this.EnableCollecting = character.EnableCollecting;

                this.Equipments.Foreach(equipmentModel =>
                                            equipmentModel.Data = character.Equipments.GetValueOrDefault(equipmentModel.Identity));

                {
                    this.UpdateClassStats();

                    foreach (var mergePair in this.EditStats.GetCombineItems(
                                 character.EditStats,
                                 pair => pair.Value.Stat,
                                 pair => pair.Key,
                                 _ => new System.Exception("Invaild merge logic (EditStats)")))
                    {
                        mergePair.Item1.Value.EditValue = mergePair.Item2.Value;
                    }
                }
            },
            false);
    }


    private void GenderChanged() =>
        this.UpdateOperation(() =>
                             {
                                 this.UpdateEquipment();
                                 this.UpdateCollectedItems();
                             });

    private void ClassChanged() =>
        this.UpdateOperation(() =>
                             {
                                 this.UpdateCharacterClass();

                                 this.UpdateEquipment();
                                 this.UpdateFreeStats();
                                 this.UpdateTalents();
                                 this.UpdateAura();
                                 this.UpdateBuffs();
                             });

    private void LevelChanged() =>
        this.UpdateOperation(() =>
                             {
                                 this.UpdateEquipment();
                                 this.UpdateFreeStats();
                                 this.UpdateTalents();
                                 this.UpdateAura();
                                 this.UpdateBuffs();
                             });

    private void TalentsChanged() =>
        this.UpdateOperation(() => { this.UpdateBuffs(); });

    private void UpdateFreeStats()
    {
        var freeStats = this.freeStatCalculator.GetFreeStats(this);

        while (freeStats < 0)
        {
            var lastNotEmptyStat = this.EditStatList.Reverse().First(stat => stat.EditValue > 1);

            var delta = lastNotEmptyStat.EditValue > -freeStats ? -freeStats : lastNotEmptyStat.EditValue - 1;

            lastNotEmptyStat.EditValue -= delta;
            freeStats += delta;
        }
    }

    private void UpdateTalents()
    {
        this.Talents = this.Talents.Where(tal => this.Class.IsSubsetOf(tal.Branch.Class)).ToList();

        if (this.freeTalentCalculator.GetFreeTalents(this) < 0)
        {
            this.Talents = this.talentCalculator.GetLimitedTalents(this).ToList();
        }
    }

    private void UpdateEquipment()
    {
        this.Equipments.Foreach(equipmentModel =>
                                {
                                    if (equipmentModel.Data != null)
                                    {
                                        var data = equipmentModel.Data;

                                        if (!data.Equipment.IsAllowed(this, equipmentModel.Identity.Slot))
                                        {
                                            equipmentModel.Data = null;
                                        }
                                        else if (data.StampVariant.Maybe(s => this.stampService.IsAllowedStamp(
                                                                                  s.Stamp,
                                                                                  data.Equipment,
                                                                                  this.Class)
                                                                              == false))
                                        {
                                            equipmentModel.Data = new CharacterEquipmentData
                                                                  {
                                                                      Active = data.Active,
                                                                      Cards = data.Cards.ToList(),
                                                                      Equipment = data.Equipment,
                                                                      Upgrade = data.Upgrade,
                                                                      StampVariant = null
                                                                  };
                                        }
                                    }
                                });

        this.UpdateCharacterImage();
    }

    private void UpdateAura()
    {
        var availableAuras = this.Class.GetAuras(this.Level, this.lastVersion).ToList();

        if (this.Aura == null || !availableAuras.Contains(this.Aura))
        {
            this.Aura = availableAuras.FirstOrDefault();
        }
    }

    private void UpdateBuffs() => this.Buffs = this.GetUpdatedClassBuffs().Concat(this.GetUpdatedSharedBuffs())
                                                   .Concat(this.GetUpdatedCardBuffs()).Concat(this.GetUpdatedStampBuffs());

    private void UpdateCollectedItems() =>
        this.CollectedItems = this.CollectedItems.Where(item => item.IsAllowed(this.Gender, this.lastVersion)).ToList();

    private Dictionary<IBuff, int> GetUpdatedClassBuffs()
    {
        var newBuffs = this.Buffs.Where(buffPair => buffPair.Key.Class != null
                                                    && this.Class.IsSubsetOf(buffPair.Key.Class)
                                                    && buffPair.Key.IsAllowed(this.Level, this.Talents)).ToDictionary();

        var autoEnabledBuffs = this.Class.GetAllParents()
                                   .SelectMany(c => c.Buffs.Where(buff => !newBuffs.ContainsKey(buff)
                                                                          && buff.AutoEnabled
                                                                          && buff.IsAllowed(this.Level, this.Talents))).ToDictionary(
                                       buff => buff,
                                       buff => buff.MaxStackCount);

        return newBuffs.Concat(autoEnabledBuffs);
    }

    private Dictionary<IBuff, int> GetUpdatedSharedBuffs() => this.Buffs.Where(buffPair => buffPair.Key.IsShared()).ToDictionary();

    private Dictionary<IBuff, int> GetUpdatedCardBuffs()
    {
        var cardBuffs = this.GetCardBuffs().Pipe(Enumerable.ToHashSet);

        return this.Buffs.Where(buffPair => buffPair.Key.Card != null && cardBuffs.Contains(buffPair.Key)).ToDictionary();
    }

    private Dictionary<IBuff, int> GetUpdatedStampBuffs()
    {
        var stampBuffs = this.GetStampBuffs().Pipe(Enumerable.ToHashSet);

        return this.Buffs.Where(buffPair => buffPair.Key.Stamp != null && stampBuffs.Contains(buffPair.Key)).ToDictionary();
    }

    private void UpdateCharacterImage()
    {
        var costumeRequest =

            from model in this.Equipments

            where model.Data != null && model.Data.Equipment.IsCostume

            select model.Data.Equipment.CostumeImage;

        this.CharacterImage = costumeRequest.SingleOrDefault() ?? this.Gender.Image!;
    }

    private void UpdateClassStats()
    {
        this.PrimaryStat = this.Class.PrimaryStat;
        this.EnergyStat = this.Class.EnergyStat;

        this.PrimaryEditStat.Stat = this.Class.PrimaryStat;

        this.PrimaryDisplayStat.Stat = this.Class.PrimaryStat;
        this.EnergyDisplayStat.Stat = this.Class.EnergyStat;

        this.RestoreEnergyDisplayStat.Stat = this.Class.EnergyStat.RestoreStats[RestoreStatType.Passive];
        this.RestoreEnergyPerHitStat.Stat = this.Class.EnergyStat.RestoreStats[RestoreStatType.PerHit];
        this.RestoreEnergyPerKillStat.Stat = this.Class.EnergyStat.RestoreStats[RestoreStatType.PerKill];
    }

    private void UpdateCharacterClass()
    {
        foreach (var equipmentModel in this.Equipments)
        {
            equipmentModel.IsAllowed = this.Class.IsAllowed(equipmentModel.Identity.Slot) || equipmentModel.Identity.Slot.IsExtraSlot();
        }

        this.UpdateClassStats();

        this.MinLevel = this.Class.GetMinLevel();
        this.MaxLevel = this.Class.Specialization.MaxLevel ?? this.lastVersion.MaxLevel;

        this.Level = this.Level.MinMax(this.MinLevel, this.MaxLevel);
    }

    private void DataModelChanged(EquipmentChangeModel changeModel) =>
        this.UpdateOperation(() =>
                             {
                                 if (changeModel.Identity.Slot.IsPrimarySlot() && changeModel.IsDoubleHand)
                                 {
                                     changeModel.ReverseModel!.Data = null;
                                 }

                                 this.UpdateCharacterImage();
                                 this.UpdateBuffs();
                             });

    protected override void InternalRecalculate()
    {
        this.FreeStats = this.freeStatCalculator.GetFreeStats(this);

        this.UpdateCharacterImage();
        this.UpdateCharacterClass();

        this.SetData(this.characterCalculator.Calculate(this));
    }

    private void SetData(CharacterCalculationResult calculationResult)
    {
        //this.Code = character.Code;
        this.SetValue(v => v.Code, calculationResult.Code);

        foreach (var mergePair in this.DisplayStats.Values.GetCombineItems(
                     calculationResult.Stats,
                     model => model.Stat,
                     pair => pair.Key,
                     _ => new System.Exception("Invalid merge logic (DisplayStats)")))
        {
            mergePair.Item1.Value = mergePair.Item2.Value;
            mergePair.Item1.DescriptionValue = calculationResult.StatDescriptions.GetMaybeValue(mergePair.Item1.Stat).ToNullable();
        }

        foreach (var editStatModel in this.EditStats)
        {
            editStatModel.Value.DisplayValue = (int)this.DisplayStats[editStatModel.Key].Value;
        }

        foreach (var equipmentModel in this.Equipments)
        {
            var resultInfo = calculationResult.Equipments.GetValueOrDefault(equipmentModel.Identity);

            if (resultInfo == null)
            {
                if (equipmentModel.DataModel != null)
                {
                    equipmentModel.DataModel.ResultInfo = null;
                }
            }
            else
            {
                equipmentModel.DataModel!.ResultInfo = resultInfo;
            }
        }
    }



    public void ResetStats() => this.UpdateOperation(() => this.EditStatList.Foreach(model => model.EditValue = 1), false);

    public Dictionary<TextTemplateVariableType, decimal> GetTemplateEvaluateStats() =>
        new()
        {
            { TextTemplateVariableType.Const, 1 },
            {
                TextTemplateVariableType.Attack,
                this.DisplayStats.Values.SingleOrDefault(v => v.Stat == this.settings.AttackStat).Maybe(ds => ds.Value)
            },
            {
                TextTemplateVariableType.Defense,
                this.DisplayStats.Values.SingleOrDefault(v => v.Stat == this.settings.DefenseStat).Maybe(ds => ds.Value)
            },
        };

    IReadOnlyDictionary<CharacterEquipmentIdentity, ICharacterEquipmentData> ICharacterSource.Equipments =>
        this.Equipments.Where(pair => pair.Data != null).ToDictionary(pair => pair.Identity, pair => pair.Data!);

    IReadOnlyDictionary<IStat, int> ICharacterSource.EditStats =>
        this.EditStats.Values.ToDictionary(model => model.Stat, model => model.EditValue);
}