using System.Runtime.Serialization;



using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.TransferData
{
    [DataContract]
    public class RaceSimpleDTO : DirectoryBaseDTO
    {
        [DataMember]
        public bool IsPvP;


        public RaceSimpleDTO(IRace race, IMappingService mappingService)
            : base(race, mappingService)
        {
            this.IsPvP = race.IsPvP;
        }
    }
}