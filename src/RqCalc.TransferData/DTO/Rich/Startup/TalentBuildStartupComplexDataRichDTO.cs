using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using Framework.Core;

using Anon.RQ_Calc.Domain;
using Framework.Core.Serialization;

namespace Anon.RQ_Calc.TransferData
{
    [DataContract]
    public class TalentBuildStartupComplexDataRichDTO
    {
        [DataMember]
        public SettingsSimpleDTO Settings;

        [DataMember]
        public List<ClassRichDTO> Classes;

        [DataMember]
        public List<BonusTypeSimpleDTO> BonusTypes;

        [DataMember]
        public string DefaultTalentBuildCode;


        public TalentBuildStartupComplexDataRichDTO(IMappingService mappingService)
        {
            if (mappingService == null) throw new ArgumentNullException(nameof(mappingService));

            var context = mappingService.Context;

            this.Settings = context.Settings.Pipe(s => new SettingsSimpleDTO(s, mappingService));
            this.Classes = context.DataSource.GetFullList<IClass>().ToList(@class => new ClassRichDTO(@class, mappingService));
            this.BonusTypes = context.DataSource.GetFullList<IBonusType>().ToList(bonusType => new BonusTypeSimpleDTO(bonusType, mappingService));


            this.DefaultTalentBuildCode = context.GuildTalentSerializer.Serialize(context.GetDefaultCharacter());
        }

        public TalentBuildStartupComplexDataRichDTO(IMappingService mappingService, CharacterStartupComplexDataRichDTO characterStartupComplexData)
        {
            if (mappingService == null) throw new ArgumentNullException(nameof(mappingService));

            var context = mappingService.Context;

            this.Settings = characterStartupComplexData.Settings;
            this.Classes = characterStartupComplexData.Classes;
            this.BonusTypes = characterStartupComplexData.BonusTypes;


            this.DefaultTalentBuildCode = context.GuildTalentSerializer.Serialize(context.GetDefaultCharacter());
        }
    }
}