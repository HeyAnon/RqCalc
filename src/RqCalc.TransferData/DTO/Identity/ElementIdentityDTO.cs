using System;
using System.Runtime.Serialization;



using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.TransferData
{
    [DataContract]
    public struct ElementIdentityDTO
    {
        [DataMember] 
        public int Id;

        
        public ElementIdentityDTO(int id)
        {
            this.Id = id;
        }

        public ElementIdentityDTO(IElement element)
        {
            if (element == null) throw new ArgumentNullException("elixir");

            this.Id = element.Id;
        }


        public IElement ToDomainObject(IMappingService mappingService)
        {
            return mappingService.GetById<IElement>(this.Id);
        }
    }
}