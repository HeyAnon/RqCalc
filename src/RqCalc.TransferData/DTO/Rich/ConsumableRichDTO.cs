using System.Collections.Generic;
using System.Runtime.Serialization;

using Framework.Core;


using Anon.RQ_Calc.Domain;


namespace Anon.RQ_Calc.TransferData
{
    [DataContract]
    public class ConsumableRichDTO : DirectoryBaseDTO
    {
        [DataMember]
        public List<BonusSimpleDTO> Bonuses;


        public ConsumableRichDTO(IConsumable consumable, IMappingService mappingService)
            : base(consumable, mappingService)
        {
            this.Bonuses = consumable.GetOrderedBonuses().ToList(bonus => new BonusSimpleDTO(bonus, mappingService));
        }
    }
}