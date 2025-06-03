using System.ComponentModel.DataAnnotations.Schema;

using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.DataBase.EntityFramework
{
    [Table("BonusTypeStatCondition")]
    public partial class BonusTypeStatCondition : PersistentDomainObjectBase
    {
        public virtual BonusTypeStat BonusTypeStat { get; set; }

        public virtual Event Event { get; set; }

        public virtual Class Class { get; set; }

        public virtual State State { get; set; }

        public virtual EquipmentType EquipmentType { get; set; }

        
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

        IEvent IBonusTypeStatCondition.Event => this.Event;

        IEquipmentType IBonusTypeStatCondition.EquipmentType => this.EquipmentType;

        IClass IClassObject.Class => this.Class;

        IState IBonusTypeStatCondition.State => this.State;
    }
}