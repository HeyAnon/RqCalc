using System.Runtime.Serialization;



using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.TransferData
{
    [DataContract]
    public class EquipmentElixirSimpleDTO : DirectoryBaseDTO
    {
        public EquipmentElixirSimpleDTO(IEquipmentElixir equipmentElixir, IMappingService mappingService)
            : base(equipmentElixir, mappingService)
        {
        }
    }
}