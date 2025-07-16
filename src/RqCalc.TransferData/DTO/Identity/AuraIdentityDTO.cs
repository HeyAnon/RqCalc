using System;
using System.Runtime.Serialization;



using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.TransferData
{
    [DataContract]
    public struct AuraIdentityDTO
    {
        [DataMember] 
        public int Id;

        
        public AuraIdentityDTO(int id)
        {
            this.Id = id;
        }

        public AuraIdentityDTO(IAura aura)
        {
            if (aura == null) throw new ArgumentNullException(nameof(aura));

            this.Id = aura.Id;
        }


        public IAura ToDomainObject(IMappingService mappingService)
        {
            return mappingService.GetById<IAura>(this.Id);
        }
    }
}