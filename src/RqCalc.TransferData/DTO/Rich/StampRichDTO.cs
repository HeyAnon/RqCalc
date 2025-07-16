using System.Collections.Generic;
using System.Runtime.Serialization;

using Framework.Core;


using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.TransferData
{
    [DataContract]
    public class StampRichDTO : StampSimpleDTO
    {
        [DataMember]
        public List<StampVariantRichDTO> Variants;
        
        [DataMember]
        public List<EquipmentTypeIdentityDTO> EquipmentTypes;

        [DataMember]
        public List<BuffRichDTO> Buffs;


        public StampRichDTO(IStamp stamp, IMappingService mappingService)
            : base(stamp, mappingService)
        {
            this.Variants = stamp.Variants.ToList(variant => new StampVariantRichDTO(variant, mappingService));

            this.EquipmentTypes = stamp.Equipments
                                       .WhereVersion(mappingService.Context.LastVersion)
                                       .ToList(stampEquipment => new EquipmentTypeIdentityDTO(stampEquipment.Type));

            this.Buffs = stamp.Buffs.WhereVersion(mappingService.Context.LastVersion).ToList(buff => new BuffRichDTO(buff, mappingService));
        }
    }
}