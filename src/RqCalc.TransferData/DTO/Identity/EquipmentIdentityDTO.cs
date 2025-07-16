using System;
using System.Runtime.Serialization;



using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.TransferData
{
    [DataContract]
    public struct EquipmentIdentityDTO
    {
        [DataMember] 
        public int Id;

        
        public EquipmentIdentityDTO(int id)
        {
            this.Id = id;
        }

        public EquipmentIdentityDTO(IEquipment equipment)
        {
            if (equipment == null) throw new ArgumentNullException(nameof(equipment));

            this.Id = equipment.Id;
        }


        public IEquipment ToDomainObject(IMappingService mappingService)
        {
            return mappingService.GetById<IEquipment>(this.Id);
        }
    }
}