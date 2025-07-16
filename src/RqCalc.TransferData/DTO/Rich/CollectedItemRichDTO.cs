using System.Collections.Generic;
using System.Runtime.Serialization;

using Framework.Core;


using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.TransferData
{
    [DataContract]
    public class CollectedItemRichDTO : CollectedItemSimpleDTO
    {
        [DataMember]
        public List<BonusSimpleDTO> Bonuses;


        public CollectedItemRichDTO(ICollectedItem collectedItem, IMappingService mappingService)
            : base(collectedItem, mappingService)
        {
            this.Bonuses = collectedItem.GetOrderedBonuses().ToList(bonus => new BonusSimpleDTO(bonus, mappingService));
        }
    }
}