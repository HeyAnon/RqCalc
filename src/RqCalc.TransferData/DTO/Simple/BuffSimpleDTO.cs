using System.Runtime.Serialization;

using Framework.Core;


using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.TransferData
{
    [DataContract]
    public class BuffSimpleDTO : DirectoryBaseDTO
    {
        [DataMember]
        public int MaxStackCount;

        [DataMember]
        public int Level;

        [DataMember]
        public bool AutoEnabled;

        [DataMember]
        public TalentIdentityDTO TalentCondition;

        [DataMember]
        public bool IsNegate;


        public BuffSimpleDTO(IBuff buff, IMappingService mappingService)
            : base(buff, mappingService)
        {
            this.MaxStackCount = buff.MaxStackCount;

            this.Level = buff.Level;
            
            this.AutoEnabled = buff.AutoEnabled;

            this.TalentCondition = buff.TalentCondition.Maybe(tal => new TalentIdentityDTO(tal));

            this.IsNegate = buff.IsNegate;
        }
    }
}