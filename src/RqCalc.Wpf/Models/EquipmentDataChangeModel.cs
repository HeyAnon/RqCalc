using Framework.Core;
using Framework.Reactive;
using Framework.Reactive.ObservableRecurse;

using RqCalc.Application;
using RqCalc.Application.ImageSource;
using RqCalc.Domain;
using RqCalc.Domain._Base;
using RqCalc.Domain._Extensions;
using RqCalc.Domain.Card;
using RqCalc.Domain.Equipment;
using RqCalc.Domain.Stamp;
using RqCalc.Domain.VirtualBonus;
using RqCalc.Model;

namespace RqCalc.Wpf.Models;

public class EquipmentDataChangeModel : NotifyModelBase, ICharacterEquipmentData
{
    private readonly IImageSourceService imageSourceService;

    public EquipmentDataChangeModel(
        IImageSourceService imageSourceService,
        IEquipmentService equipmentService,
        ICharacterEquipmentData characterEquipmentData)
    {
        this.imageSourceService = imageSourceService;
        this.Equipment = characterEquipmentData.Equipment;
        this.EquipmentBonuses = this.Equipment;
        this.Upgrade = characterEquipmentData.Upgrade;
        this.Cards = characterEquipmentData.Cards;
        this.StampVariant = characterEquipmentData.StampVariant;
        this.HasStamp = this.StampVariant != null;
        this.Elixir = characterEquipmentData.Elixir;

        this.DisplayName = this.Upgrade == 0 ? this.Equipment.Name : $"+{this.Upgrade} {this.Equipment.Name}";

        this.Activate = this.Equipment.IsActivate();
        this.Active = !this.Activate || characterEquipmentData.Active;

        this.Class = equipmentService.GetEquipmentClass(this.Equipment);

        this.ElixirClockImage = imageSourceService.GetStaticImage(StaticImageType.EquipmentElixirClock);

        this.UpdateResultInfo();

        this.SubscribeExplicit(rule => rule.Subscribe(model => model.ResultInfo, this.UpdateResultInfo));
    }



    public IEquipment Equipment
    {
        get => this.GetValue(v => v.Equipment);
        private set => this.SetValue(v => v.Equipment, value);
    }

    public IBonusContainer<IBonusBase> EquipmentBonuses
    {
        get => this.GetValue(v => v.EquipmentBonuses);
        private set => this.SetValue(v => v.EquipmentBonuses, value);
    }


    public int Upgrade
    {
        get => this.GetValue(v => v.Upgrade);
        private set => this.SetValue(v => v.Upgrade, value);
    }

    public IReadOnlyList<ICard?> Cards
    {
        get => this.CardList.ToArray(b => b.Card);
        private set => this.CardList = value.Select((card, i) => new EquipmentCardModel(this.imageSourceService, i) { Card = card }).ToObservableCollection();
    }


    public bool Active
    {
        get => this.GetValue(v => v.Active);
        set => this.SetValue(v => v.Active, value);
    }

    public bool Activate
    {
        get => this.GetValue(v => v.Activate);
        private set => this.SetValue(v => v.Activate, value);
    }


    public ObservableCollection<EquipmentCardModel> CardList
    {
        get => this.GetValue(v => v.CardList);
        private set => this.SetValue(v => v.CardList, value);
    }


    public IStampVariant? StampVariant
    {
        get => this.GetValue(v => v.StampVariant);
        private set => this.SetValue(v => v.StampVariant, value);
    }

    public IEquipmentElixir? Elixir
    {
        get => this.GetValue(v => v.Elixir);
        private set => this.SetValue(v => v.Elixir, value);
    }

    public IStampColor? StampColor => this.StampVariant.Maybe(sv => sv.Color);

    public IEquipmentClass Class
    {
        get => this.GetValue(v => v.Class);
        private set => this.SetValue(v => v.Class, value);
    }

    public IEquipmentResultInfo? ResultInfo
    {
        get => this.GetValue(v => v.ResultInfo);
        set => this.SetValue(v => v.ResultInfo, value);
    }

    public string DisplayName
    {
        get => this.GetValue(v => v.DisplayName);
        private set => this.SetValue(v => v.DisplayName, value);
    }

    public bool HasDefense
    {
        get => this.GetValue(v => v.HasDefense);
        private set => this.SetValue(v => v.HasDefense, value);
    }

    public bool HasAttack
    {
        get => this.GetValue(v => v.HasAttack);
        private set => this.SetValue(v => v.HasAttack, value);
    }

    public string DisplayDefense
    {
        get => this.GetValue(v => v.DisplayDefense);
        private set => this.SetValue(v => v.DisplayDefense, value);
    }

    public string DisplayAttack
    {
        get => this.GetValue(v => v.DisplayAttack);
        private set => this.SetValue(v => v.DisplayAttack, value);
    }

    public decimal AttackSpeed
    {
        get => this.GetValue(v => v.AttackSpeed);
        private set => this.SetValue(v => v.AttackSpeed, value);
    }

    public decimal Dps
    {
        get => this.GetValue(v => v.Dps);
        private set => this.SetValue(v => v.Dps, value);
    }

    public bool HasHpBonus
    {
        get => this.GetValue(v => v.HasHpBonus);
        private set => this.SetValue(v => v.HasHpBonus, value);
    }

    public bool HasAllStatBonus
    {
        get => this.GetValue(v => v.HasAllStatBonus);
        private set => this.SetValue(v => v.HasAllStatBonus, value);
    }

    public int HpBonus
    {
        get => this.GetValue(v => v.HpBonus);
        private set => this.SetValue(v => v.HpBonus, value);
    }

    public bool HasStamp
    {
        get => this.GetValue(v => v.HasStamp);
        private set => this.SetValue(v => v.HasStamp, value);
    }

    public bool HasElixir => this.Elixir != null;

    public IImage? ElixirClockImage { get; }


    private void UpdateResultInfo()
    {
        var upgradeInfo = this.ResultInfo.Maybe(info => info.Upgrade);

        {
            if (this.ResultInfo?.DynamicBonuses == null)
            {
                this.EquipmentBonuses = this.Equipment;
            }
            else
            {
                this.EquipmentBonuses = new VirtualBonusBaseContainer(
                    this.Equipment.GetOrderedBonuses()
                        .ToList(bonus => this.ResultInfo.DynamicBonuses.Bonuses.SingleOrDefault(b => b.Type == bonus.Type) ?? bonus));
            }
        }

        {
            var (mainAttack, attackSpeed, mainDefense) = this.Equipment.Info switch
            {
                null => (0, 0, 0),

                EquipmentInfo info => (0, 0, info.Defense),

                WeaponInfo info => (info.Attack, info.AttackSpeed, 0),

                DefenseWeaponInfo info => (info.Attack, info.AttackSpeed, info.Defense),
                
                _ => throw new ArgumentOutOfRangeException(nameof(this.Equipment.Info))
            };

            var (upgradeAttack, upgradeDefense, hpBonus) = upgradeInfo switch
            {
                null => (0, 0, 0),

                EquipmentUpgradeInfo info => (0, info.Defense, info.Hp),

                DefenseWeaponUpgradeInfo info => (info.Attack, info.Defense, 0),

                WeaponUpgradeInfo info => (info.Attack, 0, 0),

                _ => throw new ArgumentOutOfRangeException(nameof(upgradeInfo))
            };

            this.DisplayDefense = new[] { mainDefense, upgradeDefense }.Where(def => def != 0).Join("+");

            this.DisplayAttack = new[] { mainAttack, upgradeAttack }.Where(attack => attack != 0).Join("+");

            this.AttackSpeed = attackSpeed;

            this.Dps = ((mainAttack + upgradeAttack) * this.AttackSpeed).Normalize();

            this.HpBonus = hpBonus;
        }

        this.HasDefense = !this.DisplayDefense.IsNullOrWhiteSpace();
        this.HasAttack = !this.DisplayAttack.IsNullOrWhiteSpace();
        this.HasHpBonus = !this.HpBonus.IsDefault();
        this.HasAllStatBonus = upgradeInfo.Maybe(info => info.AllStatBonus != 0);
    }
}
