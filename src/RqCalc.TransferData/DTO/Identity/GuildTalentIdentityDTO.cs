using System;
using System.Runtime.Serialization;



using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.TransferData
{
    [DataContract]
    public struct GuildTalentIdentityDTO
    {
        [DataMember] 
        public int Id;

        
        public GuildTalentIdentityDTO(int id)
        {
            this.Id = id;
        }

        public GuildTalentIdentityDTO(IGuildTalent guildBonus)
        {
            if (guildBonus == null) throw new ArgumentNullException(nameof(guildBonus));

            this.Id = guildBonus.Id;
        }


        public IGuildTalent ToDomainObject(IMappingService mappingService)
        {
            return mappingService.GetById<IGuildTalent>(this.Id);
        }
    }
}