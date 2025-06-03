using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

using Framework.Core;

using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.DataBase.EntityFramework
{
    [Table("EquipmentBonus")]
    public partial class EquipmentBonus : PersistentDomainObjectBase
    {
        public virtual ICollection<EquipmentBonusVariable> NativeVariables { get; set; }

        public virtual Equipment Equipment { get; set; }

        public virtual BonusType Type { get; set; }


        public int OrderIndex { get; set; }
        
        public bool? Activate { get; set; }


        [Column("Equipment_Id")]
        public int? EquipmentId { get; set; }

        [Column("Type_Id")]
        public int? TypeId { get; set; }


        public override string ToString()
        {
            return $"BonusType: {this.Type.Template} | Values: {this.NativeVariables.Join(", ", v => $"{v.Value} [{v.Index}]")}";
        }
    }

    public partial class EquipmentBonus : IEquipmentBonus
    {
        private readonly Lazy<decimal[]> _lazyVariables;


        protected EquipmentBonus()
        {
            this._lazyVariables = LazyHelper.Create(() => this.NativeVariables.ToDictionary(v => v.Index, v => v.Value).ToArrayI());
        }


        public IReadOnlyList<decimal> Variables => this._lazyVariables.Value;


        IEquipment IEquipmentBonus.Equipment => this.Equipment;

        IBonusType Framework.Persistent.ITypeObject<IBonusType>.Type => this.Type;
    }
}