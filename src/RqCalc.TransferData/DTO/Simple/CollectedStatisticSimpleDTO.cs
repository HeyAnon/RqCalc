using System.Runtime.Serialization;



using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.TransferData
{
    [DataContract]
    public class CollectedStatisticSimpleDTO : DirectoryBaseDTO
    {
        public CollectedStatisticSimpleDTO(ICollectedStatistic collectedStatistic, IMappingService mappingService)
            : base(collectedStatistic, mappingService)
        {
        }
    }
}