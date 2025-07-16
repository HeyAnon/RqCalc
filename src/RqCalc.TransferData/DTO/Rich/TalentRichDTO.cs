using System.Linq;
using System.Runtime.Serialization;

using Framework.Core;


using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.TransferData
{
    [DataContract]
    public class TalentRichDTO : TalentSimpleDTO
    {
        [DataMember]
        public TalentDescriptionRichDTO MainDescription;

        [DataMember]
        public TalentDescriptionRichDTO PassiveDescription;


        public TalentRichDTO(ITalent talent, IMappingService mappingService)
            : base(talent, mappingService)
        {
            this.MainDescription = new TalentDescriptionRichDTO(talent.GetMainDescription(), mappingService);
            this.PassiveDescription = talent.GetPassiveDescription().Maybe(tal => new TalentDescriptionRichDTO(tal, mappingService));
        }
    }
}