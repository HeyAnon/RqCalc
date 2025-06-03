using System;

using Framework.Core;
using Framework.Core.Serialization;

using Anon.RQ_Calc.TransferData;

namespace Rq_Calc.ServiceFacade
{
    public partial class Facade
    {
        public GuildTalentBuildStartupComplexDataRichDTO GetGuildTalentBuildStartupComplexData()
        {
            return this._guildTalentBuildStartupComplexData;
        }

        public GuildTalentBuildSourceStrictDTO GetDefaultGuildTalentBuild()
        {
            return this._defaultGuildTalentBuild;
        }

        public string CalcGuildTalentBuild(GuildTalentBuildSourceStrictDTO talentBuild)
        {
            if (talentBuild == null) throw new ArgumentNullException(nameof(talentBuild));

            return this.Evaluate((context, mappingService) =>
            {
                var domainObject = talentBuild.ToDomainObject(mappingService);

                context.Validate(domainObject);

                return context.GuildTalentSerializer.Serialize(domainObject);
            });
        }

        public GuildTalentBuildSourceStrictDTO DecryptGuildTalentBuild(string code)
        {
            if (string.IsNullOrWhiteSpace(code)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(code));

            return this.Evaluate((context, mappingService) =>
            {
                return context.GuildTalentSerializer.Deserialize(code)
                                               .Pipe(characterSource => new GuildTalentBuildSourceStrictDTO(characterSource));
            });
        }
    }
}