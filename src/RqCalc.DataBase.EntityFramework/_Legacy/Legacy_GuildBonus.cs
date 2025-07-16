using System.ComponentModel.DataAnnotations.Schema;

using Framework.Core;

using RqCalc.DataBase.EntityFramework._Base;
using RqCalc.Domain._Legacy;
using RqCalc.Domain.BonusType;

namespace RqCalc.DataBase.EntityFramework._Legacy;

[Table("Legacy_GuildBonus")]
public partial class LegacyGuildBonus : ImageDirectoryBase
{
    public virtual BonusType.BonusType Type { get; set; } = null!;


    public int Value { get; set; }

    public int MaxStackCount { get; set; }
        
        

    [Column("Type_Id")]
    public int? TypeId { get; set; }
}

public partial class LegacyGuildBonus : ILegacyGuildBonus
{
    private readonly Lazy<List<decimal>> lazyVariables;


    protected LegacyGuildBonus() => this.lazyVariables = LazyHelper.Create(() => new List<decimal> { this.Value });

    public IReadOnlyCollection<decimal> Variables => this.lazyVariables.Value;


    IBonusType Framework.Persistent.ITypeObject<IBonusType>.Type => this.Type;
}