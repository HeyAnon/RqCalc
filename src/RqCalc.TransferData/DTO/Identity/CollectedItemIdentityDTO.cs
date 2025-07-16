using System;
using System.Runtime.Serialization;



using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.TransferData
{
    [DataContract]
    public struct CollectedItemIdentityDTO
    {
        [DataMember]
        public int Id;


        public CollectedItemIdentityDTO(int id)
        {
            this.Id = id;
        }

        public CollectedItemIdentityDTO(ICollectedItem collectedItem)
        {
            if (collectedItem == null) throw new ArgumentNullException(nameof(collectedItem));

            this.Id = collectedItem.Id;
        }


        public ICollectedItem ToDomainObject(IMappingService mappingService)
        {
            return mappingService.GetById<ICollectedItem>(this.Id);
        }
    }
}