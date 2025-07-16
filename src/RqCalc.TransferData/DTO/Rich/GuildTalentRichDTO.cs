using System.Collections.Generic;
using System.Runtime.Serialization;

using Framework.Core;


using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.TransferData
{
    [DataContract]
    public class GuildTalentRichDTO : GuildTalentSimpleDTO
    {
        [DataMember]
        public List<GuildTalentVariableSimpleDTO> Variables;


        public GuildTalentRichDTO(IGuildTalent guildTalent, IMappingService mappingService)
            : base(guildTalent, mappingService)
        {
            this.Variables = guildTalent.Variables.ToList(v => new GuildTalentVariableSimpleDTO(v, mappingService));
        }
    }
}