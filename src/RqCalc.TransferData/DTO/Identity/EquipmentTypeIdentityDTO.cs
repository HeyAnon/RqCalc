using System;
using System.Runtime.Serialization;



using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.TransferData
{
    [DataContract]
    public struct EquipmentTypeIdentityDTO
    {
        [DataMember] 
        public int Id;

        
        public EquipmentTypeIdentityDTO(int id)
        {
            this.Id = id;
        }

        public EquipmentTypeIdentityDTO(IEquipmentType equipmentType)
        {
            if (equipmentType == null) throw new ArgumentNullException(nameof(equipmentType));

            this.Id = equipmentType.Id;
        }


        public IEquipmentType ToDomainObject(IMappingService mappingService)
        {
            return mappingService.GetById<IEquipmentType>(this.Id);
        }
    }
}