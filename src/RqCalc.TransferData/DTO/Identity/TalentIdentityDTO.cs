using System;
using System.Runtime.Serialization;



using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.TransferData
{
    [DataContract]
    public struct TalentIdentityDTO
    {
        [DataMember] 
        public int Id;

        
        public TalentIdentityDTO(int id)
        {
            this.Id = id;
        }

        public TalentIdentityDTO(ITalent talent)
        {
            if (talent == null) throw new ArgumentNullException(nameof(talent));

            this.Id = talent.Id;
        }


        public ITalent ToDomainObject(IMappingService mappingService)
        {
            return mappingService.GetById<ITalent>(this.Id);
        }
    }
}