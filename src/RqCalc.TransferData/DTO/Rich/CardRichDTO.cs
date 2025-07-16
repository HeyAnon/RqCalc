using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

using Framework.Core;


using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.TransferData
{
    [DataContract]
    public class CardRichDTO : CardSimpleDTO
    {
        [DataMember]
        public List<BonusSimpleDTO> Bonuses;

        [DataMember]
        public List<EquipmentSlotIdentityDTO> Slots;

        [DataMember]
        public List<EquipmentTypeIdentityDTO> Types;

        [DataMember]
        public List<TextTemplateRichDTO> BuffDescriptions;

        [DataMember]
        public List<BuffRichDTO> Buffs;


        public CardRichDTO(ICard card, IMappingService mappingService)
            : base(card, mappingService)
        {
            this.Bonuses = card.GetOrderedBonuses().ToList(bonus => new BonusSimpleDTO(bonus.ToBonusBase(), mappingService));

            this.Slots = card.GetSlots(mappingService.Context.LastVersion).ToList(slot => new EquipmentSlotIdentityDTO(slot));

            this.Types = card.GetTypes(mappingService.Context.LastVersion).ToList(type => new EquipmentTypeIdentityDTO(type));

            this.BuffDescriptions = card.BuffDescriptions.OrderBy(buff => buff.OrderIndex).ToList(cb => new TextTemplateRichDTO(cb.ToTextTemplate(), mappingService));

            this.Buffs = card.Buffs.WhereVersion(mappingService.Context.LastVersion).ToList(buff => new BuffRichDTO(buff, mappingService));
        }
    }
}