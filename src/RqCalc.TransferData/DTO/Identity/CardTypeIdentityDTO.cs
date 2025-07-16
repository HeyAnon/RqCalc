using System;
using System.Runtime.Serialization;



using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.TransferData
{
    [DataContract]
    public struct CardTypeIdentityDTO
    {
        [DataMember] 
        public int Id;

        
        public CardTypeIdentityDTO(int id)
        {
            this.Id = id;
        }

        public CardTypeIdentityDTO(ICardType cardType)
        {
            if (cardType == null) throw new ArgumentNullException("elixir");

            this.Id = cardType.Id;
        }


        public ICardType ToDomainObject(IMappingService mappingService)
        {
            return mappingService.GetById<ICardType>(this.Id);
        }
    }
}