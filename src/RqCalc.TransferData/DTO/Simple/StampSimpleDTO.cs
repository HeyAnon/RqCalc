using System.Runtime.Serialization;



using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.TransferData
{
    [DataContract]
    public class StampSimpleDTO : DirectoryBaseDTO
    {
        [DataMember]
        public int OrderIndex;

        [DataMember]
        public bool IsLegacy;


        public StampSimpleDTO(IStamp stamp, IMappingService mappingService)
            : base(stamp, mappingService)
        {
            this.OrderIndex = stamp.GetOrderIndex();
            this.IsLegacy = stamp.IsLegacy;
        }
    }
}