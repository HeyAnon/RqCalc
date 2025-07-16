using System.Runtime.Serialization;



using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.TransferData
{
    [DataContract]
    public class AuraSimpleDTO : DirectoryBaseDTO
    {
        [DataMember]
        public int Level;

        public AuraSimpleDTO(IAura aura, IMappingService mappingService)
            : base(aura, mappingService)
        {
            this.Level = aura.Level;
        }
    }
}