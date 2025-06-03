using System.Runtime.Serialization;



using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.TransferData
{
    [DataContract]
    public class EquipmentClassSimpleDTO : DirectoryBaseDTO
    {
        [DataMember]
        public int? MaxUpgradeLevel;

        [DataMember]
        public bool SharedStamp;

        public EquipmentClassSimpleDTO(IEquipmentClass equipmentClass, IMappingService mappingService)
            : base(equipmentClass, mappingService)
        {
            this.MaxUpgradeLevel = equipmentClass.MaxUpgradeLevel;
            this.SharedStamp = equipmentClass.SharedStamp;
        }
    }
}