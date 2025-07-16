using System.Runtime.Serialization;

using Anon.RQ_Calc.Domain;

using Framework.Core;


namespace Anon.RQ_Calc.TransferData
{
    [DataContract]
    public class CollectedItemSimpleDTO : DirectoryBaseDTO
    {
        [DataMember]
        public GenderIdentityDTO Gender;

        public CollectedItemSimpleDTO(ICollectedItem collectedItem, IMappingService mappingService)
            : base(collectedItem, mappingService)
        {
            this.Gender = collectedItem.Gender.Maybe(v => new GenderIdentityDTO(v));
        }
    }
}