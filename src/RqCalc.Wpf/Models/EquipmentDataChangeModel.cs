using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Anon.RQ_Calc.DataBase;
using Framework.Core;

using Framework.Reactive;
using Framework.Reactive.ObservableRecurse;

using Anon.RQ_Calc.Domain;
using Anon.RQ_Calc.Logic;

namespace Anon.RQ_Calc.WPF
{
    public class EquipmentDataChangeModel : ContextModel, ICharacterEquipmentData
    {
        public EquipmentDataChangeModel(IApplicationContext context, ICharacterEquipmentData data)
            : base (context)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));

            this.Equipment = data.Equipment;
            this.EquipmentBonuses = this.Equipment;
            this.Upgrade = data.Upgrade;
            this.Cards = data.Cards;
            this.StampVariant = data.StampVariant;
            this.HasStamp = this.StampVariant != null;
            this.Elixir = data.Elixir;
            
            this.DisplayName = this.Upgrade == 0 ? this.Equipment.Name : $"+{this.Upgrade} {this.Equipment.Name}";
            
            this.Activate = this.Equipment.IsActivate();
            this.Active = !this.Activate || data.Active;

            this.Class = this.Context.GetEquipmentClass(this.Equipment);

            this.ElixirClockImage = this.Context.DataSource.GetStaticImage(StaticImageType.EquipmentElixirClock);

            this.UpdateResultInfo();

            this.SubscribeExplicit(rule => rule.Subscribe(model => model.ResultInfo, this.UpdateResultInfo));
        }



        public IEquipment Equipment
        {
            get { return this.GetValue(v => v.Equipment); }
            private set { this.SetValue(v => v.Equipment, value); }
        }

        public IBonusContainer<IBonusBase> EquipmentBonuses
        {
            get { return this.GetValue(v => v.EquipmentBonuses); }
            private set { this.SetValue(v => v.EquipmentBonuses, value); }
        }
        

        public int Upgrade
        {
            get { return this.GetValue(v => v.Upgrade); }
            private set { this.SetValue(v => v.Upgrade, value); }
        }

        public IReadOnlyList<ICard> Cards
        {
            get { return this.CardList.ToArray(b => b.Card); }
            private set { this.CardList = value.ToObservableCollection((card, i) => new EquipmentCardModel(this.Context, i) { Card = card }); }
        }


        public bool Active
        {
            get { return this.GetValue(v => v.Active); }
            set { this.SetValue(v => v.Active, value); }
        }

        public bool Activate
        {
            get { return this.GetValue(v => v.Activate); }
            private set { this.SetValue(v => v.Activate, value); }
        }


        public ObservableCollection<EquipmentCardModel> CardList
        {
            get { return this.GetValue(v => v.CardList); }
            private set { this.SetValue(v => v.CardList, value); }
        }

        
        public IStampVariant StampVariant
        {
            get { return this.GetValue(v => v.StampVariant); }
            private set { this.SetValue(v => v.StampVariant, value); }
        }

        public IEquipmentElixir Elixir
        {
            get { return this.GetValue(v => v.Elixir); }
            private set { this.SetValue(v => v.Elixir, value); }
        }

        public IStampColor StampColor
        {
            get { return this.StampVariant.Maybe(sv => sv.Color); }
        }

        public IEquipmentClass Class
        {
            get { return this.GetValue(v => v.Class); }
            private set { this.SetValue(v => v.Class, value); }
        }

        public IEquipmentResultInfo ResultInfo
        {
            get { return this.GetValue(v => v.ResultInfo); }
            set { this.SetValue(v => v.ResultInfo, value); }
        }

        public string DisplayName
        {
            get { return this.GetValue(v => v.DisplayName); }
            private set { this.SetValue(v => v.DisplayName, value); }
        }

        public bool HasDefense
        {
            get { return this.GetValue(v => v.HasDefense); }
            private set { this.SetValue(v => v.HasDefense, value); }
        }

        public bool HasAttack
        {
            get { return this.GetValue(v => v.HasAttack); }
            private set { this.SetValue(v => v.HasAttack, value); }
        }

        public string DisplayDefense
        {
            get { return this.GetValue(v => v.DisplayDefense); }
            private set { this.SetValue(v => v.DisplayDefense, value); }
        }

        public string DisplayAttack
        {
            get { return this.GetValue(v => v.DisplayAttack); }
            private set { this.SetValue(v => v.DisplayAttack, value); }
        }

        public decimal AttackSpeed
        {
            get { return this.GetValue(v => v.AttackSpeed); }
            private set { this.SetValue(v => v.AttackSpeed, value); }
        }

        public decimal DPS
        {
            get { return this.GetValue(v => v.DPS); }
            private set { this.SetValue(v => v.DPS, value); }
        }

        public bool HasHpBonus
        {
            get { return this.GetValue(v => v.HasHpBonus); }
            private set { this.SetValue(v => v.HasHpBonus, value); }
        }

        public bool HasAllStatBonus
        {
            get { return this.GetValue(v => v.HasAllStatBonus); }
            private set { this.SetValue(v => v.HasAllStatBonus, value); }
        }

        public int HpBonus
        {
            get { return this.GetValue(v => v.HpBonus); }
            private set { this.SetValue(v => v.HpBonus, value); }
        }

        public bool HasStamp
        {
            get { return this.GetValue(v => v.HasStamp); }
            private set { this.SetValue(v => v.HasStamp, value); }
        }

        public bool HasElixir => this.Elixir != null;

        public IImage ElixirClockImage { get; }


        private void UpdateResultInfo()
        {
            var upgradeInfo = this.ResultInfo.Maybe(info => info.Upgrade);
            
            {
                if (this.ResultInfo == null || this.ResultInfo.DynamicBonuses == null)
                {
                    this.EquipmentBonuses = this.Equipment;
                }
                else
                {
                    this.EquipmentBonuses = new VirtualBonusBaseContainer
                    {
                        Bonuses = this.Equipment.GetOrderedBonuses().ToList(bonus => this.ResultInfo.DynamicBonuses.Bonuses.SingleOrDefault(b => b.Type == bonus.Type) ?? bonus)
                    };
                }
            }

            {
                var mainDefense = this.Equipment.Info.Maybe(v => v.Match(info => info.Defense, _ => 0, info => info.Defense));
                var upgradeDefense = upgradeInfo.Maybe(v => v.Match(info => info.Defense, _ => 0, info => info.Defense));

                this.DisplayDefense = new[] { mainDefense, upgradeDefense }.Where(def => def != 0).Join("+");
            }

            {
                var mainAttack = this.Equipment.Info.Maybe(v => v.Match(_ => 0, info => info.Attack, info => info.Attack));
                var upgradeAttack = upgradeInfo.Maybe(v => v.Match(_ => 0, info => info.Attack, info => info.Attack));

                this.DisplayAttack = new[] { mainAttack, upgradeAttack }.Where(attack => attack != 0).Join("+");


                this.AttackSpeed = this.Equipment.Info.Maybe(v => v.Match(_ => 0, info => info.AttackSpeed, info => info.AttackSpeed));

                this.DPS = ((mainAttack + upgradeAttack) * this.AttackSpeed).Normalize();
            }

            {
                this.HpBonus = upgradeInfo.Maybe(v => v.Match(info => info.Hp, _ => 0, _ => 0));
            }

            this.HasDefense = !this.DisplayDefense.IsNullOrWhiteSpace();
            this.HasAttack = !this.DisplayAttack.IsNullOrWhiteSpace();
            this.HasHpBonus = !this.HpBonus.IsDefault();
            this.HasAllStatBonus = upgradeInfo.Maybe(info => info.AllStatBonus != 0);
        }
    }
}