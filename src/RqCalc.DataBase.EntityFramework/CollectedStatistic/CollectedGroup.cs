using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.DataBase.EntityFramework
{
    [Table("CollectedGroup")]
    public partial class CollectedGroup : DirectoryBase
    {
        public virtual ICollection<CollectedItem> Items { get; set; }

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