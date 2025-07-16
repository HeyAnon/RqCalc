using Framework.Core;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using RqCalc.DataBase.EntityFramework._Legacy;
using RqCalc.DataBase.EntityFramework.Card;
using RqCalc.DataBase.EntityFramework.CollectedStatistic;
using RqCalc.DataBase.EntityFramework.Equipment;
using RqCalc.DataBase.EntityFramework.Formula;
using RqCalc.DataBase.EntityFramework.GuildTalent;
using RqCalc.DataBase.EntityFramework.Stamp;
using RqCalc.DataBase.EntityFramework.Talent;

namespace RqCalc.DataBase.EntityFramework._DBContext;

public partial class RqCalcDbContext (
    DbContextOptions<RqCalcDbContext> dbContextOptions,
    [FromKeyedServices(ImplementTypeResolver.Key)]ITypeResolver<Type> implementTypeResolver) : DbContext(dbContextOptions)
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLazyLoadingProxies();

        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FormulaVariable>()
            .HasOne(m => m.Formula)
            .WithMany(t => t.Variables)
            .HasForeignKey(m => m.FormulaId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<CardBonus>()
            .HasOne(m => m.Card)
            .WithMany(t => t.Bonuses)
            .HasForeignKey("Card_Id")
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<CardBonus>()
            .HasOne(m => m.MultiplyEffectCard)
            .WithMany()
            .HasForeignKey("MultiplyEffectCard_Id")
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<CardBonus>()
            .HasOne(m => m.RequiredCard)
            .WithMany()
            .HasForeignKey("RequiredCard_Id")
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<CardBonus>()
            .HasOne(m => m.NegateCard)
            .WithMany()
            .HasForeignKey("NegateCard_Id")
            .OnDelete(DeleteBehavior.Restrict);


        modelBuilder.Entity<AuraBonus>()
            .HasKey(e => new { e.AuraId, e.TypeId });

        modelBuilder.Entity<BuffBonus>()
            .HasKey(e => new { e.BuffId, e.TypeId });

        modelBuilder.Entity<CollectedItemBonus>()
            .HasKey(e => new { e.CollectedItemId, e.TypeId });

        modelBuilder.Entity<ConsumableBonus>()
            .HasKey(e => new { e.ConsumableId, e.TypeId });

        modelBuilder.Entity<ElixirBonus>()
            .HasKey(e => new { e.ElixirId, e.TypeId });

        modelBuilder.Entity<EquipmentElixirBonus>()
            .HasKey(e => new { e.EquipmentElixirId, e.TypeId });

        modelBuilder.Entity<StampVariantBonus>()
            .HasKey(e => new { e.StampVariantId, e.TypeId });

        modelBuilder.Entity<BuffDescriptionVariable>()
            .HasKey(e => new { e.Index, e.BuffDescriptionId });
        
        modelBuilder.Entity<CardEquipmentSlot>()
            .HasKey(e => new { e.CardId, e.SlotId });

        modelBuilder.Entity<CardEquipmentType>()
            .HasKey(e => new { e.CardId, e.TypeId });

        modelBuilder.Entity<ClassBonus>()
            .HasKey(e => new { e.TypeId, e.ClassId });

        modelBuilder.Entity<ClassLevelHpBonus>()
            .HasKey(e => new { e.Level, e.ClassId });

        modelBuilder.Entity<EquipmentCondition>()
            .HasKey(e => new { e.EquipmentId, e.ClassId });

        modelBuilder.Entity<EquipmentElixirSlot>()
            .HasKey(e => new { e.EquipmentElixirId, e.EquipmentSlotId });

        modelBuilder.Entity<EquipmentLevelForge>()
            .HasKey(e => new { e.Level, e.EquipmentLevel });

        modelBuilder.Entity<EquipmentTypeBonus>()
            .HasKey(e => new { e.EquipmentTypeId, e.TypeId });

        modelBuilder.Entity<EquipmentTypeCondition>()
            .HasKey(e => new { e.TypeId, e.ClassId });

        modelBuilder.Entity<GuildTalentVariable>()
            .HasKey(e => new { e.Index, e.TalentId, e.Points });

        modelBuilder.Entity<StampEquipment>()
            .HasKey(e => new { e.TypeId, e.StampId });

        modelBuilder.Entity<StatBonus>()
            .HasKey(e => new { e.StatId, e.TypeId });

        modelBuilder.Entity<StatSource>()
            .HasKey(e => new { e.StatId, e.FormulaId });

        modelBuilder.Entity<TalentVariable>()
            .HasKey(e => new { e.Index, e.TalentId });
    }
        

    public DbSet<Event> Events { get; set; }

    public DbSet<Elixir> Elixirs { get; set; }

    public DbSet<Consumable> Consumables { get; set; }

    public DbSet<ConsumableBonus> ConsumableBonuses { get; set; }

    public DbSet<Card.Card> Cards { get; set; }

    public DbSet<CardType> CardTypes { get; set; }

    public DbSet<Class> Classes { get; set; }

    public DbSet<ClassBonus> ClassBonuses { get; set; }

    public DbSet<Gender> Genders { get; set; }

    public DbSet<BonusType.BonusType> BonusTypes { get; set; }

    public DbSet<CardBonus> CardBonuses { get; set; }

    public DbSet<CardBonusVariable> CardBonusVariables { get; set; }

    public DbSet<CardBonusVariableCondition> CardBonusVariableConditions { get; set; }

    public DbSet<CardEquipmentSlot> CardEquipments { get; set; }

    public DbSet<Talent.Talent> Talents { get; set; }

    public DbSet<TalentBranch> TalentBranches { get; set; }


    public DbSet<Buff> Buffs { get; set; }


    public DbSet<Stamp.Stamp> Stamps { get; set; }

    public DbSet<StampColor> StampColors { get; set; }

    public DbSet<StampVariant> StampVariants { get; set; }

    public DbSet<StampVariantBonus> StampVariantBonuses { get; set; }

    public DbSet<StampEquipment> StampEquipments { get; set; }

    public DbSet<EquipmentClass> EquipmentClasses { get; set; }

    public DbSet<Equipment.Equipment> Equipments { get; set; }

    public DbSet<EquipmentBonus> EquipmentBonuses { get; set; }

    public DbSet<EquipmentSlot> EquipmentSlots { get; set; }

    public DbSet<EquipmentType> EquipmentTypes { get; set; }

    public DbSet<EquipmentCondition> EquipmentConditions { get; set; }

    public DbSet<StaticImage> StaticImages { get; set; }

    public DbSet<Stat> Stats { get; set; }

    public DbSet<State> States { get; set; }

    public DbSet<Setting> Settings { get; set; }

    public DbSet<LegacyGuildBonus> GuildBonuses { get; set; }

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

    public DbSet<GuildTalent.GuildTalent> GuildTalents { get; set; }

    public DbSet<GuildTalentBonus> GuildTalentBonuses { get; set; }

    public DbSet<GuildTalentBonusVariable> GuildTalentBonusVariables { get; set; }

    public DbSet<GuildTalentBranch> GuildTalentBranches { get; set; }

    public DbSet<GuildTalentVariable> GuildTalentVariables { get; set; }
}