using System;
using System.Runtime.Serialization;



using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.TransferData
{
    [DataContract]
    public struct CardIdentityDTO
    {
        [DataMember] 
        public int Id;

        
        public CardIdentityDTO(int id)
        {
            this.Id = id;
        }

        public CardIdentityDTO(ICard card)
        {
            if (card == null) throw new ArgumentNullException(nameof(card));

            this.Id = card.Id;
        }


        public ICard ToDomainObject(IMappingService mappingService)
        {
            return mappingService.GetById<ICard>(this.Id);
        }
    }
}