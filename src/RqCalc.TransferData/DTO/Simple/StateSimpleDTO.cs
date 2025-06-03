using System.Runtime.Serialization;



using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.TransferData
{
    [DataContract]
    public class StateSimpleDTO : DirectoryBaseDTO
    {
        public StateSimpleDTO(IState state, IMappingService mappingService)
            : base(state, mappingService)
        {
        }
    }
}