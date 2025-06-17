using System.ComponentModel.DataAnnotations.Schema;
using RqCalc.DataBase.EntityFramework._Base;
using RqCalc.DataBase.EntityFramework.Equipment;
using RqCalc.Domain.CollectedStatistic;

namespace RqCalc.DataBase.EntityFramework.CollectedStatistic
{
    [Table("CollectedGroup")]
    public partial class CollectedGroup : DirectoryBase
    {
        public virtual HashSet<CollectedItem> Items { get; set; }

        public virtual EquipmentType EquipmentType { get; set; }

        public virtual CollectedStatistic Statistic { get; set; }
        

        public int OrderIndex { get; set; }

        [Column("EquipmentType_Id")]
        public int? EquipmentTypeId { get; set; }

        [Column("Statistic_Id")]
        public int StatisticId { get; set; }
    }

    public partial class CollectedGroup : ICollectedGroup
    {
        IEnumerable<ICollectedItem> ICollectedGroup.Items => this.Items;

        ICollectedStatistic ICollectedGroup.Statistic => this.Statistic;
    }
}