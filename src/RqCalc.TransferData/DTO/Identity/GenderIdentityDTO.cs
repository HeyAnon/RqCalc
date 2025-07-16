using System;
using System.Runtime.Serialization;



using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.TransferData
{
    [DataContract]
    public struct GenderIdentityDTO
    {
        [DataMember] 
        public int Id;

        
        public GenderIdentityDTO(int id)
        {
            this.Id = id;
        }

        public GenderIdentityDTO(IGender gender)
        {
            if (gender == null) throw new ArgumentNullException(nameof(gender));

            this.Id = gender.Id;
        }


        public IGender ToDomainObject(IMappingService mappingService)
        {
            return mappingService.GetById<IGender>(this.Id);
        }
    }
}