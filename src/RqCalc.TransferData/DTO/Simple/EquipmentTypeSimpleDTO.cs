using System.Runtime.Serialization;



using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.TransferData
{
    [DataContract]
    public class EquipmentTypeSimpleDTO : DirectoryBaseDTO
    {
        [DataMember]
        public WeaponTypeInfo WeaponInfo;


        public EquipmentTypeSimpleDTO(IEquipmentType equipmentType, IMappingService mappingService)
            : base(equipmentType, mappingService)
        {
            this.WeaponInfo = equipmentType.WeaponInfo;
        }
    }
}