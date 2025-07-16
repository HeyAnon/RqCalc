using System;
using System.Runtime.Serialization;



using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.TransferData
{
    [DataContract]
    public struct CollectedStatisticIdentityDTO
    {
        [DataMember]
        public int Id;


        public CollectedStatisticIdentityDTO(int id)
        {
            this.Id = id;
        }

        public CollectedStatisticIdentityDTO(ICollectedStatistic collectedStatistic)
        {
            if (collectedStatistic == null) throw new ArgumentNullException(nameof(collectedStatistic));

            this.Id = collectedStatistic.Id;
        }


        public ICollectedStatistic ToDomainObject(IMappingService mappingService)
        {
            return mappingService.GetById<ICollectedStatistic>(this.Id);
        }
    }
}