using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using Framework.Core;


using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.TransferData
{
    [DataContract]
    public class TalentDescriptionRichDTO
    {
        [DataMember] 
        public TextTemplateRichDTO Body;

        [DataMember]
        public List<TextTemplateRichDTO> Buffs;


        public TalentDescriptionRichDTO(ITalentDescription talentDescription, IMappingService mappingService)
        {
            if (talentDescription == null) throw new ArgumentNullException(nameof(talentDescription));
            if (mappingService == null) throw new ArgumentNullException(nameof(mappingService));

            this.Body = new TextTemplateRichDTO(talentDescription.Body, mappingService);
            this.Buffs = talentDescription.Buffs.ToList(template => new TextTemplateRichDTO(template, mappingService));
        }
    }
}