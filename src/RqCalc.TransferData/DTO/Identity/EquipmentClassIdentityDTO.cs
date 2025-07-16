using System;
using System.Runtime.Serialization;



using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.TransferData
{
    [DataContract]
    public struct EquipmentClassIdentityDTO
    {
        [DataMember] 
        public int Id;

        
        public EquipmentClassIdentityDTO(int id)
        {
            this.Id = id;
        }

        public EquipmentClassIdentityDTO(IEquipmentClass equipmentClass)
        {
            if (equipmentClass == null) throw new ArgumentNullException(nameof(equipmentClass));

            this.Id = equipmentClass.Id;
        }


        public IEquipmentClass ToDomainObject(IMappingService mappingService)
        {
            return mappingService.GetById<IEquipmentClass>(this.Id);
        }
    }
}