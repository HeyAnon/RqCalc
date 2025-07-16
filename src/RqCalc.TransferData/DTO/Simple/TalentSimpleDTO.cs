using System.Runtime.Serialization;



using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.TransferData
{
    [DataContract]
    public class TalentSimpleDTO : DirectoryBaseDTO
    {
        [DataMember]
        public int VIndex;

        [DataMember]
        public int HIndex;

        [DataMember]
        public bool Active;

        [DataMember]
        public string EquipmentCondition;


        public TalentSimpleDTO(ITalent talent, IMappingService mappingService)
            : base(talent, mappingService)
        {
            this.VIndex = talent.VIndex;
            this.HIndex = talent.HIndex;
            this.Active = talent.Active;
            this.EquipmentCondition = talent.EquipmentCondition;
        }
    }
}