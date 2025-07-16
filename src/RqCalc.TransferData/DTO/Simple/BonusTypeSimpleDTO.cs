using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using Framework.Core;


using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.TransferData
{
    [DataContract]
    public class BonusTypeSimpleDTO
    {
        [DataMember]
        public int Id;

        [DataMember]
        public string Template;

        [DataMember]
        public List<StatIdentityDTO> FilterStats;

        [DataMember]
        public List<BonusTypeVariableSimpleDTO> Variables;


        public BonusTypeSimpleDTO(IBonusType bonusType, IMappingService mappingService)
        {
            if (bonusType == null) throw new ArgumentNullException(nameof(bonusType));
            if (mappingService == null) throw new ArgumentNullException(nameof(mappingService));

            this.Id = bonusType.Id;
            this.Template = bonusType.Template;
            this.FilterStats = bonusType.GetFilterStats().ToList(stat => new StatIdentityDTO(stat));
            this.Variables = bonusType.Variables.ToList(var => new BonusTypeVariableSimpleDTO(var, mappingService));
        }
    }

    [DataContract]
    public class BonusTypeVariableSimpleDTO
    {
        [DataMember]
        public int Index;

        [DataMember]
        public bool HasSign;

        [DataMember]
        public bool IsDynamic;


        public BonusTypeVariableSimpleDTO(IBonusTypeVariable bonusTypeVariable, IMappingService mappingService)
        {
            if (bonusTypeVariable == null) throw new ArgumentNullException(nameof(bonusTypeVariable));
            if (mappingService == null) throw new ArgumentNullException(nameof(mappingService));

            this.Index = bonusTypeVariable.Index;
            this.HasSign = bonusTypeVariable.HasSign;
            this.IsDynamic = bonusTypeVariable.IsDynamic();
        }
    }
}