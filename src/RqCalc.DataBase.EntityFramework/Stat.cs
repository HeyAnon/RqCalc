using System.ComponentModel.DataAnnotations.Schema;

using Framework.Core;
using Framework.Persistent;

using RqCalc.DataBase.EntityFramework._Base;
using RqCalc.Domain;
using RqCalc.Domain._Base;
using RqCalc.Domain.Formula;

namespace RqCalc.DataBase.EntityFramework;

[Table("Stat")]
public partial class Stat : ImageDirectoryBase
{
    public virtual HashSet<StatSource> NativeSources { get; set; } = null!;

    public virtual HashSet<StatBonus> Bonuses { get; set; } = null!;

    public virtual HashSet<Stat> Children { get; set; } = null!;


    public virtual Race? Race { get; set; }

    public virtual Element? Element { get; set; }

    public virtual Stat? Parent { get; set; }

    public virtual Formula.Formula? DescriptionFormula { get; set; }


    public virtual StatType Type { get; set; }


    public bool? IsMelee { get; set; }

    public int? RoundDigits { get; set; }

    public bool IsEditable { get; set; }

    public decimal DefaultValue { get; set; }

    public bool IsPercent { get; set; }

    public string? ProgressName { get; set; }

    public int OrderIndex { get; set; }

    public string? DescriptionTemplate { get; set; }

    [Column("DescriptionFormula_Id")]
    public int? DescriptionFormulaId { get; set; }


    [Column("Race_Id")]
    public int? RaceId { get; set; }

    [Column("Element_Id")]
    public int? ElementId { get; set; }

    [Column("Parent_Id")]
    public int? ParentId { get; set; }
}

public partial class Stat : IStat
{
    private readonly Lazy<Dictionary<RestoreStatType, IStat>> lazyRestoreStats;

    private readonly Lazy<Formula.Formula[]> lazySources;


    public Stat()
    {
        this.lazyRestoreStats = LazyHelper.Create(() => this.Children.Where(c => c.Type < 0).ToDictionary(stat => (RestoreStatType)stat.Type, stat => (IStat)stat));

        this.lazySources = LazyHelper.Create(() => this.NativeSources.Where(s => s.Formula.Enabled).ToArray(s => s.Formula));
    }




    public Dictionary<RestoreStatType, IStat> RestoreStats => this.lazyRestoreStats.Value;

    public IReadOnlyCollection<IFormula> Sources => this.lazySources.Value; // For wpf binging


    IReadOnlyCollection<IStatBonus> IBonusContainer<IStatBonus>.Bonuses => this.Bonuses;

    IRace? IStat.Race => this.Race;

    IElement? IStat.Element => this.Element;

    IStat? IParentSource<IStat>.Parent => this.Parent;


    IReadOnlyDictionary<RestoreStatType, IStat> IStat.RestoreStats => this.RestoreStats;

    IFormula? IStat.DescriptionFormula => this.DescriptionFormula;
}