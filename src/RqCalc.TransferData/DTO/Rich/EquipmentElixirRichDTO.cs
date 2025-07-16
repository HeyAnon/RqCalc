using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Linq;

using Framework.Core;


using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.TransferData
{
    [DataContract]
    public class EquipmentElixirRichDTO : EquipmentElixirSimpleDTO
    {
        [DataMember]
        public List<EquipmentSlotIdentityDTO> Slots;

        [DataMember]
        public List<BonusSimpleDTO> Bonuses;

        
        public EquipmentElixirRichDTO(IEquipmentElixir equipmentElixir, IMappingService mappingService)
            : base(equipmentElixir, mappingService)
        {
            this.Slots = equipmentElixir.Slots.ToList(slot => new EquipmentSlotIdentityDTO(slot));

            this.Bonuses = equipmentElixir.GetOrderedBonuses().ToList(bonus => new BonusSimpleDTO(bonus, mappingService));
        }
    }
}