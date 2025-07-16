using System;
using System.Runtime.Serialization;



using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.TransferData
{
    [DataContract]
    public struct EquipmentElixirIdentityDTO
    {
        [DataMember]
        public int Id;


        public EquipmentElixirIdentityDTO(int id)
        {
            this.Id = id;
        }

        public EquipmentElixirIdentityDTO(IEquipmentElixir equipmentElixir)
        {
            if (equipmentElixir == null) throw new ArgumentNullException(nameof(equipmentElixir));

            this.Id = equipmentElixir.Id;
        }


        public IEquipmentElixir ToDomainObject(IMappingService mappingService)
        {
            return mappingService.GetById<IEquipmentElixir>(this.Id);
        }
    }
}