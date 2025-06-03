using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;


using Framework.Core;

using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.TransferData
{
    [DataContract]
    public class EquipmentRichDTO : EquipmentSimpleDTO
    {
        [DataMember]
        public List<ClassIdentityDTO> Classes;

        [DataMember]
        public List<BonusSimpleDTO> Bonuses;

        [DataMember]
        public CardIdentityDTO PrimaryCard;


        public EquipmentRichDTO(IEquipment equipment, IMappingService mappingService)
            : base(equipment, mappingService)
        {
            this.Classes = equipment.Conditions.ToList(cond => new ClassIdentityDTO(cond.Class));

            this.Bonuses = equipment.GetOrderedBonuses().ToList(bonus => new BonusSimpleDTO(bonus, mappingService));

            this.PrimaryCard = equipment.PrimaryCard.Maybe(card => new CardIdentityDTO(card));
        }
    }
}