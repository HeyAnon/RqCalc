using System;
using System.Data.Common;
using System.Data.Entity;

using Framework.Core;

namespace Anon.RQ_Calc.DataBase.EntityFramework
{
    [DbConfigurationType(typeof(RQDBConfiguration))]
    public partial class RQDBContext : DbContext
    {
        private readonly ITypeResolver<Type> _implementTypeResolver;


        public RQDBContext(DbConnection existingConnection, bool contextOwnsConnection, ITypeResolver<Type> implementTypeResolver)
            : base(existingConnection, contextOwnsConnection)
        {
            this._implementTypeResolver = implementTypeResolver ?? throw new ArgumentNullException(nameof(implementTypeResolver));
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FormulaVariable>()
                        .HasRequired(m => m.Formula)
                        .WithMany(t => t.Variables)
                        .HasForeignKey(m => m.FormulaId)
                        .WillCascadeOnDelete(false);

            modelBuilder.Entity<CardBonus>()
                        .HasRequired(m => m.Card)
                        .WithMany(t => t.Bonuses)
                        .HasForeignKey(m => m.CardId)
                        .WillCascadeOnDelete(false);
        }
        

        public DbSet<Event> Events { get; set; }

        public DbSet<Elixir> Elixirs { get; set; }

        public DbSet<Consumable> Consumables { get; set; }

        public DbSet<ConsumableBonus> ConsumableBonuses { get; set; }

        public DbSet<Card> Cards { get; set; }

        public DbSet<CardType> CardTypes { get; set; }

        public DbSet<Class> Classes { get; set; }

        public DbSet<ClassBonus> ClassBonuses { get; set; }

        public DbSet<Gender> Genders { get; set; }

        public DbSet<BonusType> BonusTypes { get; set; }

        public DbSet<CardBonus> CardBonuses { get; set; }

        public DbSet<CardBonusVariable> CardBonusVariables { get; set; }

        public DbSet<CardBonusVariableCondition> CardBonusVariableConditions { get; set; }

        public DbSet<CardEquipmentSlot> CardEquipments { get; set; }

        public DbSet<Talent> Talents { get; set; }

        public DbSet<TalentBranch> TalentBranches { get; set; }


        public DbSet<Buff> Buffs { get; set; }


        public DbSet<Stamp> Stamps { get; set; }

        public DbSet<StampColor> StampColors { get; set; }

        public DbSet<StampVariant> StampVariants { get; set; }

        public DbSet<StampVariantBonus> StampVariantBonuses { get; set; }

        public DbSet<StampEquipment> StampEquipments { get; set; }

        public DbSet<EquipmentClass> EquipmentClasses { get; set; }

        public DbSet<Equipment> Equipments { get; set; }

        public DbSet<EquipmentBonus> EquipmentBonuses { get; set; }

        public DbSet<EquipmentSlot> EquipmentSlots { get; set; }

        public DbSet<EquipmentType> EquipmentTypes { get; set; }

        public DbSet<EquipmentCondition> EquipmentConditions { get; set; }

        public DbSet<StaticImage> StaticImages { get; set; }

        public DbSet<Stat> Stats { get; set; }

        public DbSet<State> States { get; set; }

        public DbSet<Setting> Settings { get; set; }

        public DbSet<Legacy_GuildBonus> GuildBonuses { get; set; }

        public DbSet<ClassLevelHpBonus> ClassLevelHpBonuses { get; set; }

        public DbSet<EquipmentForge> EquipmentForges { get; set; }

        public DbSet<EquipmentLevelForge> EquipmentLevelForges { get; set; }

        public DbSet<TalentBuffDescription> TalentBuffs { get; set; }

        public DbSet<BuffDescription> BuffDescriptions { get; set; }

        public DbSet<BuffDescriptionVariable> BuffDescriptionVariables { get; set; }

        public DbSet<TalentVariable> TalentVariables { get; set; }

        public DbSet<Image> Images { get; set; }

        public DbSet<EquipmentElixir> EquipmentElixirs { get; set; }

        public DbSet<EquipmentElixirBonus> EquipmentElixirBonuses { get; set; }

        public DbSet<EquipmentElixirSlot> EquipmentElixirSlots { get; set; }
        
        public DbSet<CollectedGroup> CollectedGroups { get; set; }

        public DbSet<CollectedItem> CollectedItems { get; set; }

        public DbSet<CollectedItemBonus> CollectedItemBonuses { get; set; }
        
        public DbSet<Pet> Pets { get; set; }

        public DbSet<GuildTalent> GuildTalents { get; set; }

        public DbSet<GuildTalentBonus> GuildTalentBonuses { get; set; }

        public DbSet<GuildTalentBonusVariable> GuildTalentBonusVariables { get; set; }

        public DbSet<GuildTalentBranch> GuildTalentBranches { get; set; }

        public DbSet<GuildTalentVariable> GuildTalentVariables { get; set; }
    }
}