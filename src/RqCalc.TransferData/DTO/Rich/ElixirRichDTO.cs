using System.Collections.Generic;
using System.Runtime.Serialization;

using Framework.Core;


using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.TransferData
{
    [DataContract]
    public class ElixirRichDTO : ElixirSimpleDTO
    {
        [DataMember]
        public List<BonusSimpleDTO> Bonuses;


        public ElixirRichDTO(IElixir elixir, IMappingService mappingService)
            : base(elixir, mappingService)
        {
            this.Bonuses = elixir.GetOrderedBonuses().ToList(bonus => new BonusSimpleDTO(bonus, mappingService));
        }
    }
}