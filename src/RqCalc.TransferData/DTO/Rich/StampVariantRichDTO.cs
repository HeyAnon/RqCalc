using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using Framework.Core;


using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.TransferData
{
    [DataContract]
    public class StampVariantRichDTO
    {
        [DataMember]
        public StampColorIdentityDTO Color;

        [DataMember]
        public List<BonusSimpleDTO> Bonuses;


        public StampVariantRichDTO(IStampVariant variant, IMappingService mappingService)
        {
            if (variant == null) throw new ArgumentNullException(nameof(variant));
            if (mappingService == null) throw new ArgumentNullException(nameof(mappingService));

            this.Color = new StampColorIdentityDTO(variant.Color);

            this.Bonuses = variant.GetOrderedBonuses().ToList(bonus => new BonusSimpleDTO(bonus, mappingService));
        }
    }
}