using System;
using System.Runtime.Serialization;



using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.TransferData
{
    [DataContract]
    public struct StampColorIdentityDTO
    {
        [DataMember] 
        public int Id;

        
        public StampColorIdentityDTO(int id)
        {
            this.Id = id;
        }

        public StampColorIdentityDTO(IStampColor stampColor)
        {
            if (stampColor == null) throw new ArgumentNullException(nameof(stampColor));

            this.Id = stampColor.Id;
        }


        public IStampColor ToDomainObject(IMappingService mappingService)
        {
            return mappingService.GetById<IStampColor>(this.Id);
        }
    }
}