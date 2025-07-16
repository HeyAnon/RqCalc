using System.ComponentModel.DataAnnotations.Schema;

using Framework.Core;

using RqCalc.DataBase.EntityFramework._Base;
using RqCalc.Domain.BonusType;
using RqCalc.Domain.Equipment;

namespace RqCalc.DataBase.EntityFramework.Equipment;

[Table("EquipmentBonus")]
public partial class EquipmentBonus : PersistentDomainObjectBase
{
    public virtual HashSet<EquipmentBonusVariable> NativeVariables { get; set; } = null!;

    public virtual Equipment Equipment { get; set; } = null!;

    public virtual BonusType.BonusType Type { get; set; } = null!;


    public int OrderIndex { get; set; }
        
    public bool? Activate { get; set; }


    [Column("Equipment_Id")]
    public int? EquipmentId { get; set; }

    [Column("Type_Id")]
    public int? TypeId { get; set; }


    public override string ToString() => $"BonusType: {this.Type.Template} | Values: {this.NativeVariables.Join(", ", v => $"{v.Value} [{v.Index}]")}";
}

public partial class EquipmentBonus : IEquipmentBonus
{
    private readonly Lazy<decimal[]> lazyVariables;


    protected EquipmentBonus() => this.lazyVariables = LazyHelper.Create(() => this.NativeVariables.ToDictionary(v => v.Index, v => v.Value).ToArrayI());

    public IReadOnlyCollection<decimal> Variables => this.lazyVariables.Value;


    IEquipment IEquipmentBonus.Equipment => this.Equipment;

    IBonusType Framework.Persistent.ITypeObject<IBonusType>.Type => this.Type;
}