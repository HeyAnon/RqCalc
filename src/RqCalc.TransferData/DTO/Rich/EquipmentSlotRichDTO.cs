using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Linq;

using Framework.Core;


using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.TransferData
{
    [DataContract]
    public class EquipmentSlotRichDTO : EquipmentSlotSimpleDTO
    {
        [DataMember]
        public List<EquipmentTypeRichDTO> Types;

        public EquipmentSlotRichDTO(IEquipmentSlot equipmentSlot, IMappingService mappingService)
            : base(equipmentSlot, mappingService)
        {
            this.Types = equipmentSlot.Types.ToList(@type => new EquipmentTypeRichDTO(@type, mappingService));
        }
    }
}