using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

using Framework.Core;

using Anon.RQ_Calc.Domain;


namespace Anon.RQ_Calc.DataBase.EntityFramework
{
    [Table("EquipmentElixir")]
    public partial class EquipmentElixir : ImageDirectoryBase
    {
        public virtual ICollection<EquipmentElixirBonus> Bonuses { get; set; }

        public virtual ICollection<EquipmentElixirSlot> NativeSlots { get; set; }
        

        public virtual Version StartVersion { get; set; }

        public virtual Version EndVersion { get; set; }



        [Column("StartVersion_Id")]
        public int? StartVersionId { get; set; }

        [Column("EndVersion_Id")]
        public int? EndVersionId { get; set; }
    }


    public partial class EquipmentElixir : IEquipmentElixir
    {
        private readonly Lazy<EquipmentSlot[]> _lazySlots;

        public EquipmentElixir()
        {
            this._lazySlots = LazyHelper.Create(() => this.NativeSlots.ToArray(ns => ns.EquipmentSlot));
        }


        public IEnumerable<IEquipmentSlot> Slots => this._lazySlots.Value;


        IEnumerable<IEquipmentElixirBonus> IBonusContainer<IEquipmentElixirBonus>.Bonuses => this.Bonuses;

        IVersion IVersionObject.StartVersion => this.StartVersion;

        IVersion IVersionObject.EndVersion => this.EndVersion;
    }
}