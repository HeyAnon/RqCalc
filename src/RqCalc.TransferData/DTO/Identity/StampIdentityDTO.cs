using System;
using System.Runtime.Serialization;



using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.TransferData
{
    [DataContract]
    public struct StampIdentityDTO
    {
        [DataMember] 
        public int Id;

        
        public StampIdentityDTO(int id)
        {
            this.Id = id;
        }

        public StampIdentityDTO(IStamp stamp)
        {
            if (stamp == null) throw new ArgumentNullException(nameof(stamp));

            this.Id = stamp.Id;
        }


        public IStamp ToDomainObject(IMappingService mappingService)
        {
            return mappingService.GetById<IStamp>(this.Id);
        }
    }
}