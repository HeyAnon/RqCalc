using System.Runtime.Serialization;



using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.TransferData
{
    [DataContract]
    public class ElixirSimpleDTO : DirectoryBaseDTO
    {
        [DataMember]
        public int OrderIndex;

        [DataMember]
        public bool IsLegacy;


        public ElixirSimpleDTO(IElixir elixir, IMappingService mappingService)
            : base(elixir, mappingService)
        {
            this.OrderIndex = elixir.GetOrderIndex();
            this.IsLegacy = elixir.IsLegacy;
        }
    }
}