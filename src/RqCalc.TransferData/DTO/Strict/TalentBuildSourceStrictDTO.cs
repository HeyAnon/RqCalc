using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using Framework.Core;


using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.TransferData
{
    [DataContract]
    public class TalentBuildSourceStrictDTO
    {
        [DataMember]
        public ClassIdentityDTO Class;
        
        [DataMember]
        public int Level;
        
        [DataMember]
        public List<TalentIdentityDTO> Talents;
        

        public TalentBuildSourceStrictDTO()
        {
        }

        public TalentBuildSourceStrictDTO(ITalentBuildSource characterSource)
        {
            if (characterSource == null) throw new ArgumentNullException(nameof(characterSource));

            this.Class = new ClassIdentityDTO(characterSource.Class);
            this.Level = characterSource.Level;

            this.Talents = characterSource.Talents.ToList(t => new TalentIdentityDTO(t));
        }


        public ITalentBuildSource ToDomainObject(IMappingService mappingService)
        {
            if (mappingService == null) throw new ArgumentNullException(nameof(mappingService));

            return new TalentBuildSource
            {
                Class = this.Class.ToDomainObject(mappingService),

                Level = this.Level,

                Talents = this.Talents.ToList(v => v.ToDomainObject(mappingService))
            };
        }
    }
}