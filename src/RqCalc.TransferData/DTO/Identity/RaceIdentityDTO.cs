using System;
using System.Runtime.Serialization;



using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.TransferData
{
    [DataContract]
    public struct RaceIdentityDTO
    {
        [DataMember] 
        public int Id;

        
        public RaceIdentityDTO(int id)
        {
            this.Id = id;
        }

        public RaceIdentityDTO(IRace race)
        {
            if (race == null) throw new ArgumentNullException("elixir");

            this.Id = race.Id;
        }


        public IRace ToDomainObject(IMappingService mappingService)
        {
            return mappingService.GetById<IRace>(this.Id);
        }
    }
}