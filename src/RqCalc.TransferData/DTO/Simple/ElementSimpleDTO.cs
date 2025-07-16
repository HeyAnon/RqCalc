using System.Runtime.Serialization;



using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.TransferData
{
    [DataContract]
    public class ElementSimpleDTO : DirectoryBaseDTO
    {
        public ElementSimpleDTO(IElement element, IMappingService mappingService)
            : base(element, mappingService)
        {

        }
    }
}