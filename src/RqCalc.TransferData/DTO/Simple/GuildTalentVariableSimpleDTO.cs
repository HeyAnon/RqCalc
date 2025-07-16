using System;
using System.Runtime.Serialization;



using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.TransferData
{
    [DataContract]
    public class GuildTalentVariableSimpleDTO
    {
        [DataMember]
        public decimal Value;

        [DataMember]
        public int Points;

        [DataMember]
        public int Index;


        public GuildTalentVariableSimpleDTO(IGuildTalentVariable guildTalentVariable, IMappingService mappingService)
        {
            if (guildTalentVariable == null) throw new ArgumentNullException(nameof(guildTalentVariable));
            if (mappingService == null) throw new ArgumentNullException(nameof(mappingService));

            this.Value = guildTalentVariable.Value;
            this.Points = guildTalentVariable.Points;
            this.Index = guildTalentVariable.Index;
        }
    }
}