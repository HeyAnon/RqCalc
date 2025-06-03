using System;
using System.ServiceModel;

using Anon.RQ_Calc.Domain;
using Anon.RQ_Calc.Logic;
using Anon.RQ_Calc.TransferData;

using Framework.DataBase;

namespace Rq_Calc.ServiceFacade
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Single)]
    public partial class Facade : FacadeBase<ServiceFacadeEnvironment, MappingService, IApplicationContext, IDataSource<IPersistentDomainObjectBase>>, IFacade
    {
        private readonly MappingService _mappingService;

        private readonly CharacterStartupComplexDataRichDTO _characterStartupComplexData;

        private readonly TalentBuildStartupComplexDataRichDTO _talentBuildStartupComplexData;

        private readonly GuildTalentBuildStartupComplexDataRichDTO _guildTalentBuildStartupComplexData;

        private readonly CharacterSourceStrictDTO _defaultCharacter;

        private readonly TalentBuildSourceStrictDTO _defaultTalentBuild;

        private readonly GuildTalentBuildSourceStrictDTO _defaultGuildTalentBuild;


        public Facade(ServiceFacadeEnvironment environment)
            : base(environment)
        {
            this._mappingService = new MappingService(this.Environment.DefaultApplicationContext);

            {
                this._characterStartupComplexData = new CharacterStartupComplexDataRichDTO(this._mappingService);

                this._talentBuildStartupComplexData = new TalentBuildStartupComplexDataRichDTO(this._mappingService, this._characterStartupComplexData);

                this._guildTalentBuildStartupComplexData = new GuildTalentBuildStartupComplexDataRichDTO(this._mappingService, this._characterStartupComplexData);
            }

            {
                var defaultCharacter = this._mappingService.Context.GetDefaultCharacter();

                this._defaultCharacter = new CharacterSourceStrictDTO(defaultCharacter);

                this._defaultTalentBuild = new TalentBuildSourceStrictDTO(defaultCharacter);

                this._defaultGuildTalentBuild = new GuildTalentBuildSourceStrictDTO(defaultCharacter);
            }
        }


        protected override IApplicationContext GetContext()
        {
            return this.Environment.DefaultApplicationContext;
        }

        protected override MappingService GetMappingService()
        {
            return this._mappingService;
        }
    }
}