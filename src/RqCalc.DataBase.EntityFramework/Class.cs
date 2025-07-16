using System.ComponentModel.DataAnnotations.Schema;

using Framework.Core;
using Framework.Persistent;

using RqCalc.DataBase.EntityFramework._Base;
using RqCalc.DataBase.EntityFramework.Talent;
using RqCalc.Domain;
using RqCalc.Domain.Talent;

namespace RqCalc.DataBase.EntityFramework;

[Table("Class")]
public partial class Class : ImageDirectoryBase
{
    public virtual HashSet<Class> SubClasses { get; set; } = null!;

    public virtual HashSet<ClassBonus> NativeBonuses { get; set; } = null!;

    public virtual HashSet<ClassLevelHpBonus> LevelHpBonuses { get; set; } = null!;

    public virtual HashSet<TalentBranch> TalentBranches { get; set; } = null!;


    public virtual HashSet<Aura> Auras { get; set; } = null!;

    public virtual HashSet<Buff> Buffs { get; set; } = null!;


    public virtual Class? Base { get; set; }

    public virtual Stat NativePrimaryStat { get; set; } = null!;

    public virtual Stat NativeEnergyStat { get; set; } = null!;

    public virtual ClassSpecialization Specialization { get; set; } = null!;



    [Column("IsMelee")]
    public bool? NativeIsMelee { get; set; }

    [Column("AllowExtraWeapon")]
    public bool? NativeAllowExtraWeapon { get; set; }

    [Column("HpPerVitality")]
    public decimal? NativeHpPerVitality { get; set; }


    [Column("Base_Id")]
    public int? BaseId { get; set; }

    [Column("PrimaryStat_Id")]
    public int? NativePrimaryStatId { get; set; }

    [Column("EnergyStat_Id")]
    public int? NativeEnergyStatId { get; set; }

    [Column("Specialization_Id")]
    public int? SpecializationId { get; set; }
}


public partial class Class : IClass
{
    private readonly Lazy<bool> lazyAllowExtraWeapon;
        
    private readonly Lazy<bool> lazyIsMelee;

    private readonly Lazy<decimal> lazyHpPerVitality;

    private readonly Lazy<Stat> lazyPrimaryStat;

    private readonly Lazy<Stat> lazyEnergyStat;

    private readonly Lazy<IReadOnlyCollection<ClassBonus>> lazyBonuses;


    public Class()
    {
        this.lazyAllowExtraWeapon = LazyHelper.Create(() => this.GetFirst(c => c.NativeAllowExtraWeapon));

        this.lazyIsMelee = LazyHelper.Create(() => this.GetFirst(c => c.NativeIsMelee));

        this.lazyHpPerVitality = LazyHelper.Create(() => this.GetFirst(c => c.NativeHpPerVitality));

        this.lazyPrimaryStat = LazyHelper.Create(() => this.GetFirst(c => c.NativePrimaryStat));

        this.lazyEnergyStat = LazyHelper.Create(() => this.GetFirst(c => c.NativeEnergyStat));

        this.lazyBonuses = LazyHelper.Create(() => this.GetMany(c => c.NativeBonuses));
    }


    public bool AllowExtraWeapon => this.lazyAllowExtraWeapon.Value;

    public bool IsMelee => this.lazyIsMelee.Value;

    public decimal HpPerVitality => this.lazyHpPerVitality.Value;

    public IStat PrimaryStat => this.lazyPrimaryStat.Value;

    public IStat EnergyStat => this.lazyEnergyStat.Value;

    IReadOnlyCollection<IAura> IClass.Auras => this.Auras;

    IReadOnlyCollection<IBuff> IClass.Buffs => this.Buffs;


    public IReadOnlyCollection<IClassBonus> Bonuses => this.lazyBonuses.Value; // For wpf binging


    IReadOnlyCollection<ITalentBranch> IClass.TalentBranches => this.TalentBranches;


    IEnumerable<IClass> IChildrenSource<IClass>.Children => this.SubClasses;

    IReadOnlyCollection<IClassLevelHpBonus> IClass.LevelHpBonuses => this.LevelHpBonuses;

    IClassSpecialization IClass.Specialization => this.Specialization;

    IClass? IParentSource<IClass>.Parent => this.Base;


    private T GetFirst<T>(Func<Class, T> selector)
        where T : class
    {
        var request = from c in this.GetAllElements(v => v.Base)

            let val = selector(c)

            where val != null

            select val;

        return request.First();
    }

    private T GetFirst<T>(Func<Class, T?> selector)
        where T : struct 
    {
        var request = from c in this.GetAllElements(v => v.Base)

            let val = selector(c)

            where val != null

            select val.Value;

        return request.First();
    }

    private IReadOnlyCollection<T> GetMany<T>(Func<Class, IEnumerable<T>> selector, bool reverse = true)
    {
        var request = from c in this.GetAllElements(v => v.Base).Pipe(reverse, c => c.Reverse())

            from val in selector(c)

            select val;

        return request.ToList();
    }
}