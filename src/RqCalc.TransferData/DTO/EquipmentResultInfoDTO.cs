using System;
using System.Runtime.Serialization;

using Anon.RQ_Calc.Domain;

using Framework.Core;


namespace Anon.RQ_Calc.TransferData
{
    [DataContract]
    public class EquipmentResultInfoDTO
    {
        [DataMember]
        public EquipmentUpgradeInfoDTO Upgrade;

        [DataMember]
        public BonusContainerRichDTO StampVariant;

        [DataMember]
        public BonusContainerRichDTO DynamicBonuses;


        public EquipmentResultInfoDTO(IEquipmentResultInfo info, IMappingService mappingService)
        {
            if (info == null) throw new ArgumentNullException(nameof(info));
            if (mappingService == null) throw new ArgumentNullException(nameof(mappingService));
            
            this.Upgrade = info.Upgrade.Maybe(upg => new EquipmentUpgradeInfoDTO(upg));
            this.StampVariant = info.StampVariant.Maybe(stampVariant => new BonusContainerRichDTO(stampVariant, mappingService));
            this.DynamicBonuses = info.DynamicBonuses.Maybe(stampVariant => new BonusContainerRichDTO(stampVariant, mappingService));
        }
    }
}