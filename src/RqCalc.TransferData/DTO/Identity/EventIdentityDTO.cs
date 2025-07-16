using System;
using System.Runtime.Serialization;



using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.TransferData
{
    [DataContract]
    public struct EventIdentityDTO
    {
        [DataMember] 
        public int Id;

        
        public EventIdentityDTO(int id)
        {
            this.Id = id;
        }

        public EventIdentityDTO(IEvent @event)
        {
            if (@event == null) throw new ArgumentNullException(nameof(@event));

            this.Id = @event.Id;
        }


        public IEvent ToDomainObject(IMappingService mappingService)
        {
            return mappingService.GetById<IEvent>(this.Id);
        }
    }
}