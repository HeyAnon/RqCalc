using System;
using System.Runtime.Serialization;



using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.TransferData
{
    [DataContract]
    public struct StateIdentityDTO
    {
        [DataMember] 
        public int Id;

        
        public StateIdentityDTO(int id)
        {
            this.Id = id;
        }

        public StateIdentityDTO(IState state)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));

            this.Id = state.Id;
        }


        public IState ToDomainObject(IMappingService mappingService)
        {
            return mappingService.GetById<IState>(this.Id);
        }
    }
}