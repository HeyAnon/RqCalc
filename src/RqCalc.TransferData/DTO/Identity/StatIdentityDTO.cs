using System;
using System.Runtime.Serialization;



using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.TransferData
{
    [DataContract]
    public struct StatIdentityDTO
    {
        [DataMember] 
        public int Id;

        
        public StatIdentityDTO(int id)
        {
            this.Id = id;
        }

        public StatIdentityDTO(IStat stat)
        {
            if (stat == null) throw new ArgumentNullException(nameof(stat));

            this.Id = stat.Id;
        }


        public IStat ToDomainObject(IMappingService mappingService)
        {
            return mappingService.GetById<IStat>(this.Id);
        }
    }
}