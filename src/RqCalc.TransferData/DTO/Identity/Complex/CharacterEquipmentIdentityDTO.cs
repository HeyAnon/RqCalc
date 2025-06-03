using System;
using System.Runtime.Serialization;



using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.TransferData
{
    [DataContract]
    public class CharacterEquipmentIdentityDTO
    {
        [DataMember]
        public EquipmentSlotIdentityDTO Slot;
        
        [DataMember]
        public int Index;

        
        public CharacterEquipmentIdentityDTO()
        {
            
        }

        public CharacterEquipmentIdentityDTO([NotNull]CharacterEquipmentIdentity equipmentIdentity)
        {
            if (equipmentIdentity == null) throw new ArgumentNullException(nameof(equipmentIdentity));

            this.Slot = new EquipmentSlotIdentityDTO(equipmentIdentity.Slot);
            this.Index = equipmentIdentity.Index;
        }


        public CharacterEquipmentIdentity ToDomainObject(IMappingService mappingService)
        {
            if (mappingService == null) throw new ArgumentNullException(nameof(mappingService));

            return new CharacterEquipmentIdentity
            {
                Slot = this.Slot.ToDomainObject(mappingService),

                Index = this.Index,
            };
        }
    }
}