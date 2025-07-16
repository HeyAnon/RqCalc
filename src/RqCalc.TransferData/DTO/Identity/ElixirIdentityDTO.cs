using System;
using System.Runtime.Serialization;



using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.TransferData
{
    [DataContract]
    public struct ElixirIdentityDTO
    {
        [DataMember] 
        public int Id;

        
        public ElixirIdentityDTO(int id)
        {
            this.Id = id;
        }

        public ElixirIdentityDTO(IElixir elixir)
        {
            if (elixir == null) throw new ArgumentNullException(nameof(elixir));

            this.Id = elixir.Id;
        }


        public IElixir ToDomainObject(IMappingService mappingService)
        {
            return mappingService.GetById<IElixir>(this.Id);
        }
    }
}