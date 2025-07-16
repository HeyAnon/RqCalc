using System.ComponentModel.DataAnnotations.Schema;

using Framework.Core;

using RqCalc.Domain._Base;
using RqCalc.Domain.BonusType;

namespace RqCalc.DataBase.EntityFramework._Base;

public abstract partial class BonusBase
{
    public virtual BonusType.BonusType Type { get; set; } = null!;


    public decimal Value { get; set; }

        
        
    [Column("Type_Id")]
    public virtual int TypeId { get; set; }


    public override string ToString() => $"BonusType: {this.Type.Template} | Value: {this.Value}";
}

public abstract partial class BonusBase : IBonusBase
{
    private readonly Lazy<List<decimal>> lazyVariables;


    protected BonusBase() => this.lazyVariables = LazyHelper.Create(() => new List<decimal> { this.Value });

    public IReadOnlyCollection<decimal> Variables => this.lazyVariables.Value;


    IBonusType Framework.Persistent.ITypeObject<IBonusType>.Type => this.Type;
}