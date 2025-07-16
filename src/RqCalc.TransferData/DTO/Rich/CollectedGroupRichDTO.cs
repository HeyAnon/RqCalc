using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

using Framework.Core;


using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.TransferData
{
    [DataContract]
    public class CollectedGroupRichDTO : DirectoryBaseDTO
    {
        [DataMember]
        public CollectedStatisticIdentityDTO Statistic;

        [DataMember]
        public List<CollectedItemRichDTO> Items;


        public CollectedGroupRichDTO(ICollectedGroup collectedGroup, IMappingService mappingService)
            : base(collectedGroup, mappingService)
        {
            this.Statistic = new CollectedStatisticIdentityDTO(collectedGroup.Statistic);

            this.Items = collectedGroup.Items.WhereVersion(mappingService.Context.LastVersion)
                                             .OrderBy(item => item.OrderIndex).ToList(item => new CollectedItemRichDTO(item, mappingService));
        }
    }
}