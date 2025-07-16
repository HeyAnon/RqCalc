using System.Collections.Generic;
using System.Runtime.Serialization;

using Framework.Core;


using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.TransferData
{
    [DataContract]
    public class BuffRichDTO : BuffSimpleDTO
    {
        [DataMember]
        public List<BuffBonusSimpleDTO> Bonuses;
        
        
        public BuffRichDTO(IBuff buff, IMappingService mappingService)
            : base(buff, mappingService)
        {
            this.Bonuses = buff.GetOrderedBonuses().ToList(bonus => new BuffBonusSimpleDTO(bonus, mappingService));
        }
    }
}