using System.Linq;
using System.Runtime.Serialization;



using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.TransferData
{
    [DataContract]
    public class GuildTalentSimpleDTO : DirectoryBaseDTO
    {
        [DataMember]
        public string DescriptionTemplate;

        [DataMember]
        public bool Active;


        public GuildTalentSimpleDTO(IGuildTalent guildTalent, IMappingService mappingService)
            : base(guildTalent, mappingService)
        {
            this.DescriptionTemplate = guildTalent.GetDescriptionTemplate();
            this.Active = guildTalent.Active;
        }
    }
}