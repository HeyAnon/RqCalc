using System.Runtime.Serialization;

using Framework.Core;


using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.TransferData
{
    [DataContract]
    public class EquipmentSlotSimpleDTO : DirectoryBaseDTO
    {
        [DataMember]
        public StateIdentityDTO State;

        [DataMember]
        public EquipmentSlotIdentityDTO ExtraSlot;

        [DataMember]
        public EquipmentSlotIdentityDTO PrimarySlot;

        [DataMember]
        public int Count;

        [DataMember]
        public bool? IsWeapon;

        [DataMember]
        public bool SharedStamp;


        public EquipmentSlotSimpleDTO(IEquipmentSlot equipmentSlot, IMappingService mappingService)
            : base (equipmentSlot, mappingService)
        {
            this.State = equipmentSlot.State.Maybe(v => new StateIdentityDTO(v));
            this.ExtraSlot = equipmentSlot.ExtraSlot.Maybe(v => new EquipmentSlotIdentityDTO(v));
            this.PrimarySlot = equipmentSlot.PrimarySlot.Maybe(v => new EquipmentSlotIdentityDTO(v));
            this.Count = equipmentSlot.Count;
            this.IsWeapon = equipmentSlot.IsWeapon;
            this.SharedStamp = equipmentSlot.SharedStamp;
        }
    }
}