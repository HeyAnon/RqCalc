using System;
using System.Runtime.Serialization;



using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.TransferData
{
    [DataContract]
    public struct EquipmentSlotIdentityDTO
    {
        [DataMember] 
        public int Id;

        
        public EquipmentSlotIdentityDTO(int id)
        {
            this.Id = id;
        }

        public EquipmentSlotIdentityDTO(IEquipmentSlot equipmentSlot)
        {
            if (equipmentSlot == null) throw new ArgumentNullException(nameof(equipmentSlot));

            this.Id = equipmentSlot.Id;
        }


        public IEquipmentSlot ToDomainObject(IMappingService mappingService)
        {
            return mappingService.GetById<IEquipmentSlot>(this.Id);
        }
    }
}