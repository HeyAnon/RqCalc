using System;
using System.Runtime.Serialization;



using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.TransferData
{
    [DataContract]
    public struct ConsumableIdentityDTO
    {
        [DataMember] 
        public int Id;

        
        public ConsumableIdentityDTO(int id)
        {
            this.Id = id;
        }

        public ConsumableIdentityDTO(IConsumable consumable)
        {
            if (consumable == null) throw new ArgumentNullException(nameof(consumable));

            this.Id = consumable.Id;
        }


        public IConsumable ToDomainObject(IMappingService mappingService)
        {
            return mappingService.GetById<IConsumable>(this.Id);
        }
    }
}