using System.Runtime.Serialization;

using Framework.Core;


using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.TransferData
{
    [DataContract]
    public class CardTypeSimpleDTO : DirectoryBaseDTO
    {
        [DataMember]
        public bool HasToolTipImage;

        [DataMember]
        public ElementIdentityDTO Element;

        [DataMember]
        public EquipmentClassIdentityDTO MaxEquipmentClass;
        
        public CardTypeSimpleDTO(ICardType cardType, IMappingService mappingService)
            : base(cardType, mappingService)
        {
            this.HasToolTipImage = cardType.HasToolTipImage;

            this.Element = cardType.Element.Maybe(el => new ElementIdentityDTO(el));
            this.MaxEquipmentClass = cardType.MaxEquipmentClass.Maybe(el => new EquipmentClassIdentityDTO(el));
        }
    }
}