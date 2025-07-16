using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

using Framework.Core;


using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.TransferData
{
    [DataContract]
    public class AuraRichDTO : AuraSimpleDTO
    {
        [DataMember]
        public List<BonusSimpleDTO> Bonuses;

        [DataMember]
        public List<BonusSimpleDTO> SharedBonusesWithoutTalents;

        [DataMember]
        public List<BonusSimpleDTO> SharedBonusesWithTalents;


        public AuraRichDTO(IAura aura, IMappingService mappingService)
            : base(aura, mappingService)
        {
            this.Bonuses = aura.GetOrderedBonuses().ToList(bonus => new BonusSimpleDTO(bonus, mappingService));

            this.SharedBonusesWithoutTalents = aura.GetBonuses(mappingService.Context.LastVersion, true, false).ToList(bonus => new BonusSimpleDTO(bonus, mappingService));

            this.SharedBonusesWithTalents = aura.GetBonuses(mappingService.Context.LastVersion, true, true).ToList(bonus => new BonusSimpleDTO(bonus, mappingService));
        }
    }
}