using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using Framework.Core;


using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.TransferData
{
    [DataContract]
    public class StatSimpleDTO  : DirectoryBaseDTO
    {
        [DataMember]
        public bool IsEditable;

        [DataMember]
        public bool IsPercent;

        [DataMember]
        public Dictionary<RestoreStatType, StatIdentityDTO> RestoreStats;

        [DataMember]
        public int? RoundDigits;

        [DataMember]
        public bool? IsMelee;
        
        [DataMember]
        public int OrderIndex;

        [DataMember]
        public string ProgressName;

        [DataMember]
        public string DescriptionTemplate;

        [DataMember] 
        public StatType Type;

        [DataMember]
        public RaceIdentityDTO Race;

        [DataMember]
        public ElementIdentityDTO Element;


        public StatSimpleDTO(IStat stat, IMappingService mappingService)
            : base (stat, mappingService)
        {
            this.IsEditable = stat.IsEditable;
            this.IsPercent = stat.IsPercent;
            this.RestoreStats = stat.RestoreStats.ChangeValue(s => new StatIdentityDTO(s));
            this.RoundDigits = stat.RoundDigits;
            this.OrderIndex = stat.OrderIndex;
            this.IsMelee = stat.IsMelee;
            this.ProgressName = stat.ProgressName;
            this.DescriptionTemplate = stat.DescriptionTemplate;
            this.Type = stat.Type;
            this.Race = stat.Race.Maybe(r => new RaceIdentityDTO(r));
            this.Element = stat.Element.Maybe(e => new ElementIdentityDTO(e));
        }
    }
}