using System.Collections.Generic;
using System.Runtime.Serialization;

using Framework.Core;


using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.TransferData
{
    [DataContract]
    public class GuildTalentBranchRichDTO : DirectoryBaseDTO
    {
        [DataMember]
        public List<GuildTalentRichDTO> Talents;

        [DataMember]
        public int MaxPoints;

        public GuildTalentBranchRichDTO(IGuildTalentBranch talentBranch, IMappingService mappingService)
            : base(talentBranch, mappingService)
        {
            this.Talents = talentBranch.Talents.ToList(t => new GuildTalentRichDTO(t, mappingService));
            this.MaxPoints = talentBranch.MaxPoints;
        }
    }
}