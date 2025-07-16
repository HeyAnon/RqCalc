using System;
using System.Runtime.Serialization;



using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.TransferData
{
    [DataContract]
    public class StampColorSimpleDTO : DirectoryBaseDTO
    {
        [DataMember]
        public string Argb;


        public StampColorSimpleDTO(IStampColor stampColor, IMappingService mappingService)
            : base(stampColor, mappingService)
        {
            this.Argb = stampColor.Argb;
        }
    }
}