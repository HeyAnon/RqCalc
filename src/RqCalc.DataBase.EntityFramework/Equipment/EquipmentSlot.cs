using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

using Framework.Core;

using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.DataBase.EntityFramework
{
    [Table("EquipmentSlot")]
    public partial class EquipmentSlot : ImageDirectoryBase
    {
        public virtual ICollection<EquipmentType> Types { get; set; }


        public virtual ICollection<EquipmentSlot> PrimarySlots { get; set; }


        public virtual State State { get; set; }

        public virtual EquipmentSlot ExtraSlot { get; set; }


        public bool SharedStamp { get; set; }

        public int Count { get; set; }

        public bool? IsWeapon { get; set; }


        [Column("State_Id")]
        public int? StateId { get; set; }

        [Column("ExtraSlot_Id")]
        public int? ExtraSlotId { get; set; }
    }

    public partial class EquipmentSlot : IEquipmentSlot
    {
        private readonly Lazy<EquipmentSlot> _lazyWeaponInfo;


        public EquipmentSlot()
        {
            this._lazyWeaponInfo = LazyHelper.Create(() => this.PrimarySlots.SingleOrDefault());
        }


        public EquipmentSlot PrimarySlot => this._lazyWeaponInfo.Value;


        IEnumerable<IEquipmentType> IEquipmentSlot.Types => this.Types;

        IState IEquipmentSlot.State => this.State;

        IEquipmentSlot IEquipmentSlot.ExtraSlot => this.ExtraSlot;

        IEquipmentSlot IEquipmentSlot.PrimarySlot => this.PrimarySlot;
    }
}