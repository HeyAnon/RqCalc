using System;
using System.Runtime.Serialization;



using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.TransferData
{
    [DataContract]
    public class GenderSimpleDTO : DirectoryBaseDTO
    {
        public GenderSimpleDTO(IGender gender, IMappingService mappingService)
            : base(gender, mappingService)
        {
        }
    }
}