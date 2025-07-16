using System.Runtime.Serialization;

using Framework.Core;


using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.TransferData
{
    [DataContract]
    public class EquipmentSimpleDTO : DirectoryBaseDTO
    {
        [DataMember]
        public GenderIdentityDTO Gender;

        [DataMember]
        public EquipmentTypeIdentityDTO Type;

        [DataMember]
        public EquipmentInfoDTO Info;

        [DataMember]
        public int Level;

        [DataMember]
        public EquipmentClassIdentityDTO Class;

        [DataMember]
        public bool IsActivate;

        [DataMember]
        public bool IsCostume;

        [DataMember]
        public bool IsPersonal;


        public EquipmentSimpleDTO(IEquipment equipment, IMappingService mappingService)
            : base(equipment, mappingService)
        {
            this.Gender = equipment.Gender.Maybe(v => new GenderIdentityDTO(v));
            this.Type = new EquipmentTypeIdentityDTO(equipment.Type);
            this.Info = equipment.Info.Maybe(info => new EquipmentInfoDTO(info));
            this.Level = equipment.Level;
            this.Class = new EquipmentClassIdentityDTO(mappingService.GetEquipmentClass(equipment));
            this.IsActivate = equipment.IsActivate();
            this.IsCostume = equipment.IsCostume;
            this.IsPersonal = equipment.IsPersonal;
        }
    }
}