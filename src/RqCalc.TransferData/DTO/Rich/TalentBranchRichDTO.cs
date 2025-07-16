using System.Collections.Generic;
using System.Runtime.Serialization;

using Framework.Core;


using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.TransferData
{
    [DataContract]
    public class TalentBranchRichDTO : DirectoryBaseDTO
    {
        [DataMember] 
        public List<TalentRichDTO> Talents;
        

        public TalentBranchRichDTO(ITalentBranch talentBranch, IMappingService mappingService)
            : base(talentBranch, mappingService)
        {
            this.Talents = talentBranch.Talents.ToList(t => new TalentRichDTO(t, mappingService));
        }
    }
}