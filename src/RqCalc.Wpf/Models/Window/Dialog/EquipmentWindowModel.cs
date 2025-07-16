using Framework.Core;
using Framework.DataBase;
using Framework.Reactive;
using Framework.Reactive.ObservableRecurse;

using RqCalc.Application.ImageSource;
using RqCalc.Domain;
using RqCalc.Domain._Base;
using RqCalc.Domain._Extensions;
using RqCalc.Domain.Card;
using RqCalc.Domain.Equipment;
using RqCalc.Domain.Stamp;
using RqCalc.Model;
using RqCalc.Wpf._Extensions;
using RqCalc.Wpf.Models._Base;
using RqCalc.Wpf.Models.Window.Dialog._Base;
using RqCalc.Application;
using RqCalc.Model._Extensions;

namespace RqCalc.Wpf.Models.Window.Dialog;

public class EquipmentWindowModel : NotifyModelBase, ICharacterEquipmentData, IClearModel, IEvaluateClientContext
{
    private readonly IModelFactory modelFactory;

    private readonly IVersion lastVersion;

    private readonly IDataSource<IPersistentDomainObjectBase> dataSource;

    private readonly IEquipmentSlotService equipmentSlotService;

    private readonly IImageSourceService imageSourceService;

    private readonly IStampService stampService;

    private readonly IEquipmentService equipmentService;

    private readonly IEquipmentSlot slot;

    private readonly IEquipmentType? reverseWeaponType;


    public EquipmentWindowModel(
        IModelFactory modelFactory,
        IVersion lastVersion,
        IDataSource<IPersistentDomainObjectBase> dataSource,
        IEquipmentSlotService equipmentSlotService,
        IImageSourceService imageSourceService,
        IStampService stampService,
        IEquipmentService equipmentService,
        IReadOnlyDictionary<TextTemplateVariableType, decimal> evaluateStats,
        ICharacterSourceBase character,
        IEquipmentSlot slot,
        ICharacterEquipmentData? currentData,
        IEquipment? reverseEquipment)
    {
        this.Character = character;
        this.EvaluateStats = evaluateStats;
        this.modelFactory = modelFactory;
        this.lastVersion = lastVersion;
        this.dataSource = dataSource;
        this.equipmentSlotService = equipmentSlotService;
        this.imageSourceService = imageSourceService;
        this.stampService = stampService;
        this.equipmentService = equipmentService;
        this.slot = slot;
        this.reverseWeaponType = reverseEquipment.Maybe(e => e.Type.Slot.IsWeapon == true, e => e.Type);



        this.Active = currentData.Maybe(data => data.Equipment.IsActivate() && data.Active);
        this.Equipment = currentData.Maybe(data => data.Equipment);

        var allowWeapon = this.slot.IsWeapon == true || (this.slot.IsExtraSlot() && this.Character.Class.AllowExtraWeapon);
        var allowEquip = !this.slot.IsWeapon.GetValueOrDefault() && character.Class.IsAllowed(this.slot);


        this.AllowChangeType = allowWeapon == allowEquip;

        this.IsWeapon = !allowEquip || this.Equipment.Maybe(e => e.Type.Slot, this.slot).IsWeapon == true;

        this.CardList = [];
        this.UpdateCards();

        this.Cards = currentData.Maybe(data => data.Cards);
        this.StampVariant = currentData.Maybe(data => data.StampVariant);
        this.Upgrade = currentData.Maybe(data => data.Upgrade);
        this.Elixir = currentData.Maybe(data => data.Elixir);


        this.RefreshEquipmentSource();
        this.UpdateStampVariant();
        this.UpdateUpgrades();

        this.SubscribeExplicit(
            rule => rule.Subscribe(model => model.IsWeapon, this.RefreshEquipmentSource),
            rule => rule.Subscribe(model => model.Equipment, this.UpdateEquipment),
            rule => rule.Subscribe(model => model.StampVariant, this.UpdateStampVariant));

        this.DelegateRaiseSubscribe(m => m.SingleElixir, this, m => m.SelectedSingleEquipmentElixir);
        this.DelegateRaiseSubscribe(m => m.Elixir, this, m => m.SelectedSingleEquipmentElixir);
    }

    public CardWindowModel CreateCardWindowModel(
        int cardIndex,
        ICard? startupCard) => this.modelFactory.CreateCardWindowModel(
        this.EvaluateStats,
        this.Equipment!.Type,
        cardIndex,
        this.Character.Class,
        startupCard,
        this.equipmentService.GetEquipmentClass(this.Equipment));

    public StampWindowModel CreateStampWindowModel() =>
        this.modelFactory.CreateStampWindowModel(this.Equipment!, this.Character.Class, this.StampVariant);

    public IReadOnlyDictionary<TextTemplateVariableType, decimal> EvaluateStats
    {
        get => this.GetValue(v => v.EvaluateStats);
        private set => this.SetValue(v => v.EvaluateStats, value);
    }

    public ICharacterSourceBase Character { get => this.GetValue(v => v.Character); private set => this.SetValue(v => v.Character, value); }


    public bool Active { get => this.GetValue(v => v.Active); private set => this.SetValue(v => v.Active, value); }

    public ObservableCollection<IEquipment> Equipments
    {
        get => this.GetValue(v => v.Equipments);
        set => this.SetValue(v => v.Equipments, value);
    }

    public IEquipment? Equipment { get => this.GetValue(v => v.Equipment); set => this.SetValue(v => v.Equipment, value); }

    public bool AllowChangeType
    {
        get => this.GetValue(v => v.AllowChangeType);
        private set => this.SetValue(v => v.AllowChangeType, value);
    }

    public bool IsWeapon { get => this.GetValue(v => v.IsWeapon); set => this.SetValue(v => v.IsWeapon, value); }

    public int Upgrade { get => this.GetValue(v => v.Upgrade); set => this.SetValue(v => v.Upgrade, value); }

    public IStampVariant? StampVariant { get => this.GetValue(v => v.StampVariant); set => this.SetValue(v => v.StampVariant, value); }

    public IEquipmentElixir? Elixir { get => this.GetValue(v => v.Elixir); set => this.SetValue(v => v.Elixir, value); }

    public IEquipmentElixir? SingleElixir
    {
        get => this.GetValue(v => v.SingleElixir);
        private set => this.SetValue(v => v.SingleElixir, value);
    }

    public bool HasSingleElixir
    {
        get => this.GetValue(v => v.HasSingleElixir);
        private set => this.SetValue(v => v.HasSingleElixir, value);
    }

    public bool SelectedSingleEquipmentElixir
    {
        get => this.Elixir != null && this.Elixir == this.SingleElixir;
        set => this.Elixir = value ? this.SingleElixir : null;
    }



    public bool HasUpgrade => this.slot.IsWeapon != null;

    private bool IsExtraWeapon => this.IsWeapon && this.slot.IsExtraSlot();

    private IEquipmentSlot ActualSlot => this.IsExtraWeapon ? this.slot.PrimarySlot! : this.slot;

    public int MaxUpgradeLevel
    {
        get => this.GetValue(v => v.MaxUpgradeLevel);
        private set => this.SetValue(v => v.MaxUpgradeLevel, value);
    }

    public bool HasStamp { get => this.GetValue(v => v.HasStamp); private set => this.SetValue(v => v.HasStamp, value); }

    public IStampColor? StampColor { get => this.GetValue(v => v.StampColor); private set => this.SetValue(v => v.StampColor, value); }

    public IStamp? Stamp { get => this.GetValue(v => v.Stamp); private set => this.SetValue(v => v.Stamp, value); }

    public IImage? StampImage { get => this.GetValue(v => v.StampImage); private set => this.SetValue(v => v.StampImage, value); }


    public IReadOnlyList<ICard?>? Cards
    {
        get => this.CardList.ToArray(model => model.Card);
        set
        {
            if (value == null)
            {
                this.CardList.Foreach(model => model.Card = null);
            }
            else
            {
                this.CardList.ForeachS(value, (model, card) => model.Card = card);
            }
        }
    }


    public ObservableCollection<EquipmentCardModel> CardList
    {
        get => this.GetValue(v => v.CardList);
        private set => this.SetValue(v => v.CardList, value);
    }


    public void Clear() => this.Equipment = null;

    public bool CloseDialog => true;


    private void UpdateStampVariant()
    {
        this.HasStamp = this.StampVariant != null;
        this.Stamp = this.StampVariant.Maybe(v => v.Stamp);
        this.StampColor = this.StampVariant.Maybe(v => v.Color);
        this.StampImage = this.StampColor.Maybe(
            stampColor => stampColor.BigImage,
            () => this.imageSourceService.GetStaticImage(StaticImageType.EmptyStamp));
    }

    private void UpdateEquipment()
    {
        this.UpdateCards();

        this.StampVariant.Maybe(sv =>
                                {
                                    if (this.Equipment == null
                                        || this.stampService.IsAllowedStamp(sv.Stamp, this.Equipment, this.Character.Class) == false)
                                    {
                                        this.StampVariant = null;
                                    }
                                });

        this.UpdateUpgrades();
    }

    private void UpdateUpgrades()
    {

        this.MaxUpgradeLevel = this.Equipment.Maybe(e => this.equipmentService.GetMaxUpgradeLevel(e));
        this.Upgrade = Math.Min(this.Upgrade, this.MaxUpgradeLevel);
    }

    private void UpdateCards()
    {
        var maxCardCount = this.equipmentSlotService.GetMaxCardCount(this.ActualSlot);

        while (this.CardList.Count > maxCardCount)
        {
            this.CardList.RemoveAt(this.CardList.Count - 1);
        }

        while (this.CardList.Count < maxCardCount)
        {
            this.CardList.Add(new EquipmentCardModel(this.imageSourceService, this.CardList.Count));
        }

        foreach (var cardModel in this.CardList)
        {
            var allowed = this.Equipment.Maybe(e => cardModel.Card.Maybe(c => c.IsAllowed(
                                                                             e.Type,
                                                                             this.lastVersion,
                                                                             this.equipmentService.GetEquipmentClass(this.Equipment!))));

            if (!allowed)
            {
                cardModel.Card = null;
            }
        }

        this.Equipment.Maybe(e => e.PrimaryCard).Maybe(card => this.CardList[0].Card = card);
    }

    private void RefreshEquipmentSource()
    {
        var request = this.dataSource
                          .GetFullList<IEquipment>()
                          .WhereVersion(this.lastVersion)
                          .Where(equipment => equipment.IsAllowed(this.Character) && equipment.Type.Slot == this.ActualSlot)
                          .Where(equipment => !this.IsWeapon
                                              || equipment.IsDoubleHand()
                                              || this.reverseWeaponType == null
                                              || equipment.Type == this.reverseWeaponType)
                          .Where(equipment => !this.slot.HasReverseSlot() || !equipment.IsDoubleHand() || this.slot.IsPrimarySlot())
                          .OrderByDescending(equipment => equipment.Level)

                          .ThenBy(equipment => equipment.Name);


        this.Equipments = request.ToObservableCollection();

        this.Equipment ??= this.Equipments.FirstOrDefault();

        {
            var elixirs = this.dataSource.GetFullList<IEquipmentElixir>().WhereVersion(this.lastVersion)
                              .Where(el => el.Slots.Contains(this.ActualSlot)).ToList();

            this.HasSingleElixir = elixirs.Count == 1;

            this.SingleElixir = this.HasSingleElixir ? elixirs.Single() : null;

            if (this.Elixir != null && !elixirs.Contains(this.Elixir))
            {
                this.Elixir = null;
            }
        }
    }
}
