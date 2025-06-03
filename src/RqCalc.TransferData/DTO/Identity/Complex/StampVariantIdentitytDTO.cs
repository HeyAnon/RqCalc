using System;
using System.Runtime.Serialization;



using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.TransferData
{
    [DataContract]
    public class StampVariantIdentitytDTO
    {
        [DataMember]
        public StampIdentityDTO Stamp;

        [DataMember]
        public StampColorIdentityDTO Color;


        public StampVariantIdentitytDTO()
        {

        }

        public StampVariantIdentitytDTO([NotNull]IStampVariant stampVariant)
        {
            if (stampVariant == null) throw new ArgumentNullException(nameof(stampVariant));

            this.Stamp = new StampIdentityDTO(stampVariant.Stamp);
            this.Color = new StampColorIdentityDTO(stampVariant.Color);
        }


        public IStampVariant ToDomainObject(IMappingService mappingService)
        {
            if (mappingService == null) throw new ArgumentNullException(nameof(mappingService));

            return mappingService.GetStampVariant(this.Stamp, this.Color);
        }
    }
}