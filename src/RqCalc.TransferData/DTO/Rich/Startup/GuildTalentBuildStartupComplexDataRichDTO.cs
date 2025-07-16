using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using Framework.Core;

using Anon.RQ_Calc.Domain;
using Framework.Core.Serialization;

namespace Anon.RQ_Calc.TransferData
{
    [DataContract]
    public class GuildTalentBuildStartupComplexDataRichDTO
    {
        [DataMember]
        public List<GuildTalentBranchRichDTO> GuildTalentBranches;

        [DataMember]
        public string DefaultGuildTalentBuildCode;


        public GuildTalentBuildStartupComplexDataRichDTO(IMappingService mappingService)
        {
            if (mappingService == null) throw new ArgumentNullException(nameof(mappingService));

            var context = mappingService.Context;

            this.GuildTalentBranches = context.DataSource.GetFullList<IGuildTalentBranch>().ToList(gb => new GuildTalentBranchRichDTO(gb, mappingService));

            this.DefaultGuildTalentBuildCode = context.GuildTalentSerializer.Serialize(context.GetDefaultCharacter());
        }

        public GuildTalentBuildStartupComplexDataRichDTO(IMappingService mappingService, CharacterStartupComplexDataRichDTO characterStartupComplexData)
        {
            if (mappingService == null) throw new ArgumentNullException(nameof(mappingService));

            var context = mappingService.Context;

            this.GuildTalentBranches = characterStartupComplexData.GuildTalentBranches;

            this.DefaultGuildTalentBuildCode = context.GuildTalentSerializer.Serialize(context.GetDefaultCharacter());
        }
    }
}