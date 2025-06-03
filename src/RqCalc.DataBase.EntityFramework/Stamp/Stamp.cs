using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.DataBase.EntityFramework
{
    [Table("Stamp")]
    public partial class Stamp : DirectoryBase
    {
        public virtual ICollection<Buff> Buffs { get; set; }

        public virtual ICollection<StampVariant> Variants { get; set; }

        public virtual ICollection<StampEquipment> Equipments { get; set; }


        public bool IsLegacy { get; set; }
    }

    public partial class Stamp : IStamp
    {
        IEnumerable<IBuff> IStamp.Buffs => this.Buffs;

        IEnumerable<IStampVariant> IStamp.Variants => this.Variants;

        IEnumerable<IStampEquipment> IStamp.Equipments => this.Equipments;
    }
}