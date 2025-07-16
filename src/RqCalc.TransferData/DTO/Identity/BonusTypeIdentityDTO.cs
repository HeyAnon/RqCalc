using System;
using System.Runtime.Serialization;



using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.TransferData
{
    [DataContract]
    public struct BonusTypeIdentityDTO
    {
        [DataMember]
        public int Id;


        public BonusTypeIdentityDTO(int id)
        {
            this.Id = id;
        }

        public BonusTypeIdentityDTO(IBonusType bonusType)
        {
            if (bonusType == null) throw new ArgumentNullException(nameof(bonusType));

            this.Id = bonusType.Id;
        }


        public IBonusType ToDomainObject(IMappingService mappingService)
        {
            return mappingService.GetById<IBonusType>(this.Id);
        }
    }
}