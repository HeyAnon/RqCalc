using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;

using Framework.Core;


using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.TransferData
{
    [DataContract]
    public class CharacterResultRichDTO
    {
        [DataMember]
        public string Code;

        //[DataMember]
        //public string TalentCode;


        [DataMember]
        public Dictionary<StatIdentityDTO, decimal> Stats;
        
        [DataMember]
        public Dictionary<CharacterEquipmentIdentityDTO, EquipmentResultInfoDTO> Equipments;

        [DataMember]
        public Dictionary<StatIdentityDTO, decimal> StatDescriptions;


        public CharacterResultRichDTO(ICharacterResult characterResult, IMappingService mappingService)
        {
            if (characterResult == null) throw new ArgumentNullException(nameof(characterResult));
            if (mappingService == null) throw new ArgumentNullException(nameof(mappingService));
            
            this.Code = characterResult.Code;
            //this.TalentCode = characterResult.TalentCode;

            this.Stats = characterResult.Stats.ChangeKey(stat => new StatIdentityDTO(stat));
            this.Equipments = characterResult.Equipments.ToDictionary(pair => new CharacterEquipmentIdentityDTO(pair.Key), pair => new EquipmentResultInfoDTO(pair.Value, mappingService));
            this.StatDescriptions = characterResult.StatDescriptions.ChangeKey(stat => new StatIdentityDTO(stat));
        }
    }
}