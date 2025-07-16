using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using Framework.Core;


using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.TransferData
{
    [DataContract]
    public class GuildTalentBuildSourceStrictDTO
    {
        [DataMember]
        public Dictionary<GuildTalentIdentityDTO, int> GuildTalents;


        public GuildTalentBuildSourceStrictDTO()
        {
        }

        public GuildTalentBuildSourceStrictDTO(IGuildTalentBuildSource characterSource)
        {
            if (characterSource == null) throw new ArgumentNullException(nameof(characterSource));

            this.GuildTalents = characterSource.GuildTalents.ChangeKey(guildBonus => new GuildTalentIdentityDTO(guildBonus));
        }


        public IGuildTalentBuildSource ToDomainObject(IMappingService mappingService)
        {
            if (mappingService == null) throw new ArgumentNullException(nameof(mappingService));

            return new GuildTalentBuildSource
            {
                GuildTalents = this.GuildTalents.ChangeKey(v => v.ToDomainObject(mappingService))
            };
        }
    }
}