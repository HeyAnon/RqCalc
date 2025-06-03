using System.Collections.Generic;
using System.Runtime.Serialization;


using Framework.Core;

using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.TransferData
{
    [DataContract]
    public class EquipmentTypeRichDTO : EquipmentTypeSimpleDTO
    {
        [DataMember]
        public List<ClassIdentityDTO> Classes;


        public EquipmentTypeRichDTO(IEquipmentType equipmentType, IMappingService mappingService)
            : base(equipmentType, mappingService)
        {
            this.Classes = equipmentType.Conditions.ToList(cond => new ClassIdentityDTO(cond.Class));
        }
    }
}