using System;

using Framework.Core;
using Framework.Core.Serialization;

using Anon.RQ_Calc.TransferData;

namespace Rq_Calc.ServiceFacade
{
    public partial class Facade
    {
        public TalentBuildStartupComplexDataRichDTO GetTalentBuildStartupComplexData()
        {
            return this._talentBuildStartupComplexData;
        }

        public TalentBuildSourceStrictDTO GetDefaultTalentBuild()
        {
            return this._defaultTalentBuild;
        }

        public string CalcTalentBuild(TalentBuildSourceStrictDTO talentBuild)
        {
            if (talentBuild == null) throw new ArgumentNullException(nameof(talentBuild));

            return this.Evaluate((context, mappingService) =>
            {
                var domainObject = talentBuild.ToDomainObject(mappingService);

                context.Validate(domainObject);

                return context.TalentSerializer.Serialize(domainObject);
            });
        }

        public TalentBuildSourceStrictDTO DecryptTalentBuild(string code)
        {
            if (string.IsNullOrWhiteSpace(code)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(code));

            return this.Evaluate((context, mappingService) =>
            {
                return context.TalentSerializer.Deserialize(code)
                                               .Pipe(characterSource => new TalentBuildSourceStrictDTO(characterSource));
            });
        }
    }
}