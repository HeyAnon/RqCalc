using System.Runtime.Serialization;



using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.TransferData
{
    [DataContract]
    public class EventSimpleDTO : DirectoryBaseDTO
    {
        public EventSimpleDTO(IEvent @event, IMappingService mappingService)
            : base(@event, mappingService)
        {
        }
    }
}