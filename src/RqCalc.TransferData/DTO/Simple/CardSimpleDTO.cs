using System.Runtime.Serialization;

using Framework.Core;


using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.TransferData
{
    [DataContract]
    public class CardSimpleDTO : DirectoryBaseDTO
    {
        [DataMember]
        public CardTypeIdentityDTO Type;

        [DataMember]
        public EquipmentClassIdentityDTO MinEquipmentClass;

        [DataMember]
        public int OrderIndex;

        [DataMember]
        public bool IsLegacy;

        [DataMember]
        public CardGroupInfo Group;

        [DataMember]
        public bool? IsSingleHandWeapon;

        [DataMember]
        public bool? IsMeleeWeapon;

        public CardSimpleDTO(ICard card, IMappingService mappingService)
            : base(card, mappingService)
        {
            this.Type = new CardTypeIdentityDTO(card.Type);
            this.MinEquipmentClass = card.MinEquipmentClass.Maybe(e => new EquipmentClassIdentityDTO(e));

            this.OrderIndex = card.GetOrderIndex();
            this.IsLegacy = card.IsLegacy;
            this.Group = card.Group;
            this.IsSingleHandWeapon = card.IsSingleHandWeapon;
            this.IsMeleeWeapon = card.IsMeleeWeapon;
        }
    }
}