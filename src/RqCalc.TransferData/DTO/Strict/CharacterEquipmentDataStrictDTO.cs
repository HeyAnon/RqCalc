using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using Framework.Core;


using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.TransferData
{
    [DataContract]
    public class CharacterEquipmentDataStrictDTO
    {
        [DataMember]
        public EquipmentIdentityDTO Equipment;
        
        [DataMember]
        public List<CardIdentityDTO> Cards;

        [DataMember]
        public StampVariantIdentitytDTO StampVariant;
        
        [DataMember]
        public int Upgrade;

        [DataMember]
        public bool Active;

        [DataMember]
        public EquipmentElixirIdentityDTO Elixir;

        public CharacterEquipmentDataStrictDTO()
        {
            
        }

        public CharacterEquipmentDataStrictDTO([NotNull]ICharacterEquipmentData equipmentData)
        {
            if (equipmentData == null) throw new ArgumentNullException(nameof(equipmentData));

            this.Equipment = new EquipmentIdentityDTO(equipmentData.Equipment);
            this.Cards = equipmentData.Cards.ToList(c => c.Maybe(v => new CardIdentityDTO(v)));
            this.StampVariant = equipmentData.StampVariant.Maybe(v => new StampVariantIdentitytDTO(v));
            this.Active = equipmentData.Active;
            this.Upgrade = equipmentData.Upgrade;
            this.Elixir = equipmentData.Elixir.Maybe(elixir => new EquipmentElixirIdentityDTO(elixir));
        }


        public CharacterEquipmentData ToDomainObject(IMappingService mappingService)
        {
            if (mappingService == null) throw new ArgumentNullException(nameof(mappingService));

            return new CharacterEquipmentData
            {
                Equipment = this.Equipment.ToDomainObject(mappingService),

                Cards = this.Cards.ToList(card => card.ToDomainObject(mappingService)),

                StampVariant = this.StampVariant.Maybe(sv => sv.ToDomainObject(mappingService)),
                
                Active = this.Active,

                Upgrade = this.Upgrade,

                Elixir = this.Elixir.ToDomainObject(mappingService)
            };
        }
    }
}