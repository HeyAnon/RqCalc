using System.Collections.Generic;
using System.Runtime.Serialization;

using Anon.RQ_Calc.Domain;

using Framework.Core;


namespace Anon.RQ_Calc.TransferData
{
    [DataContract]
    public class BonusContainerRichDTO
    {
        [DataMember]
        public List<BonusSimpleDTO> Bonuses;


        public BonusContainerRichDTO(IBonusContainer<IBonusBase> bonusContainer, IMappingService mappingService)
        {
            this.Bonuses = bonusContainer.Bonuses.ToList(bonus => new BonusSimpleDTO(bonus, mappingService));
        }
    }
}