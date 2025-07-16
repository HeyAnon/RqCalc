using System.ComponentModel.DataAnnotations.Schema;

using RqCalc.DataBase.EntityFramework._Base;
using RqCalc.DataBase.EntityFramework.Equipment;
using RqCalc.Domain;
using RqCalc.Domain.BonusType;
using RqCalc.Domain.Equipment;

namespace RqCalc.DataBase.EntityFramework.BonusType;

[Table("BonusTypeStatCondition")]
public partial class BonusTypeStatCondition : PersistentDomainObjectBase
{
    public virtual BonusTypeStat BonusTypeStat { get; set; } = null!;

    public virtual Event? Event { get; set; }

    public virtual Class? Class { get; set; }

    public virtual State? State { get; set; }

    public virtual EquipmentType? EquipmentType { get; set; }


    public bool? IsMaxLevel { get; set; }

    public bool? PairEquipment { get; set; }

    public bool? LostControl { get; set; }


    [Column("BonusTypeStat_Id")]
    public int? BonusTypeStatId { get; set; }

    [Column("Event_Id")]
    public int? EventId { get; set; }
        
    [Column("EquipmentType_Id")]
    public int? EquipmentTypeId { get; set; }

    [Column("Class_Id")]
    public int? ClassId { get; set; }

    [Column("State_Id")]
    public int? StateId { get; set; }
}

public partial class BonusTypeStatCondition : IBonusTypeStatCondition
{
    IBonusTypeStat IBonusTypeStatCondition.BonusTypeStat => this.BonusTypeStat;

    IEvent? IBonusTypeStatCondition.Event => this.Event;

    IEquipmentType? IBonusTypeStatCondition.EquipmentType => this.EquipmentType;

    IClass? IBonusTypeStatCondition.Class => this.Class;

    IState? IBonusTypeStatCondition.State => this.State;
}