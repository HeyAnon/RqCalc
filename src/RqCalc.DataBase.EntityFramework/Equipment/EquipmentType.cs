using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

using Anon.RQ_Calc.Domain;
using Framework.Core;

namespace Anon.RQ_Calc.DataBase.EntityFramework
{
    [Table("EquipmentType")]
    public partial class EquipmentType : DirectoryBase
    {
        public virtual ICollection<EquipmentTypeBonus> Bonuses { get; set; }
        
        public virtual ICollection<EquipmentTypeCondition> Conditions { get; set; }

        public virtual ICollection<Equipment> Equipments { get; set; }


        public virtual EquipmentSlot Slot { get; set; }
        

        public bool? IsMeleeWeapon { get; set; }

        public bool? IsSingleHandWeapon { get; set; }


        [Column("Slot_Id")]
        public int? SlotId { get; set; }
    }

    public partial class EquipmentType : IEquipmentType
    {
        private readonly Lazy<WeaponTypeInfo> _lazyWeaponInfo;


        public EquipmentType()
        {
            this._lazyWeaponInfo = LazyHelper.Create(this.GetWeaponInfo);
        }


        public WeaponTypeInfo WeaponInfo => this._lazyWeaponInfo.Value;


        IEnumerable<IEquipmentTypeBonus> IBonusContainer<IEquipmentTypeBonus>.Bonuses => this.Bonuses;

        IEnumerable<IEquipmentTypeCondition> IEquipmentType.Conditions => this.Conditions;

        IEnumerable<IEquipment> IEquipmentType.Equipments => this.Equipments;

        IEquipmentSlot IEquipmentType.Slot => this.Slot;


        private WeaponTypeInfo GetWeaponInfo()
        {
            if (this.IsMeleeWeapon != null && this.IsSingleHandWeapon != null)
            {
                return new WeaponTypeInfo(this.IsMeleeWeapon.Value, this.IsSingleHandWeapon.Value);
            }
            else if (this.IsMeleeWeapon == null && this.IsSingleHandWeapon == null)
            {
                return null;
            }
            else
            {
                throw new Exception("Invalid WeaponInfo");
            }
        }
    }
}