using System;
using System.Runtime.Serialization;



using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.TransferData
{
    [DataContract]
    public struct BuffIdentityDTO
    {
        [DataMember] 
        public int Id;

        
        public BuffIdentityDTO(int id)
        {
            this.Id = id;
        }

        public BuffIdentityDTO(IBuff buff)
        {
            if (buff == null) throw new ArgumentNullException(nameof(buff));

            this.Id = buff.Id;
        }


        public IBuff ToDomainObject(IMappingService mappingService)
        {
            return mappingService.GetById<IBuff>(this.Id);
        }
    }
}