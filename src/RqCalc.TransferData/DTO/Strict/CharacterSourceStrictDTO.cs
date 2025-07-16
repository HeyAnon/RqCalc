using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;

using Framework.Core;


using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.TransferData
{
    [DataContract]
    public class CharacterSourceStrictDTO
    {
        [DataMember]
        public ClassIdentityDTO Class;

        [DataMember]
        public GenderIdentityDTO Gender;

        [DataMember]
        public int Level;

        [DataMember]
        public StateIdentityDTO State;

        [DataMember]
        public EventIdentityDTO Event;

        [DataMember]
        public Dictionary<StatIdentityDTO, int> EditStats;

        [DataMember]
        public Dictionary<CharacterEquipmentIdentityDTO, CharacterEquipmentDataStrictDTO> Equipments;

        [DataMember]
        public ElixirIdentityDTO Elixir;

        [DataMember]
        public List<ConsumableIdentityDTO> Consumables;

        [DataMember]
        public Dictionary<GuildTalentIdentityDTO, int> GuildTalents;

        [DataMember]
        public AuraIdentityDTO Aura;

        [DataMember]
        public Dictionary<AuraIdentityDTO, bool> SharedAuras;

        [DataMember]
        public Dictionary<BuffIdentityDTO, int> Buffs;

        [DataMember]
        public List<TalentIdentityDTO> Talents;

        [DataMember]
        public List<CollectedItemIdentityDTO> CollectedItems;

        [DataMember]
        public bool EnableElixir;

        [DataMember]
        public bool EnableConsumables;

        [DataMember]
        public bool EnableGuildTalents;

        [DataMember]
        public bool EnableAura;

        [DataMember]
        public bool EnableBuffs;

        [DataMember]
        public bool EnableTalents;

        [DataMember]
        public bool LostControl;

        [DataMember]
        public bool EnableCollecting;


        public CharacterSourceStrictDTO()
        {
        }

        public CharacterSourceStrictDTO(ICharacterSource characterSource)
        {
            if (characterSource == null) throw new ArgumentNullException(nameof(characterSource));

            this.Class = new ClassIdentityDTO(characterSource.Class);
            this.Gender = new GenderIdentityDTO(characterSource.Gender);
            this.Level = characterSource.Level;
            this.State = new StateIdentityDTO(characterSource.State);
            this.Event = characterSource.Event.Maybe(v => new EventIdentityDTO(v));

            this.EditStats = characterSource.EditStats.ChangeKey(stat => new StatIdentityDTO(stat));
            this.Equipments = characterSource.Equipments.ToDictionary(pair => new CharacterEquipmentIdentityDTO(pair.Key), pair => new CharacterEquipmentDataStrictDTO(pair.Value));
            this.Elixir = characterSource.Elixir.Maybe(v => new ElixirIdentityDTO(v));
            this.Consumables = characterSource.Consumables.ToList(c => new ConsumableIdentityDTO(c));
            this.GuildTalents = characterSource.GuildTalents.ChangeKey(guildBonus => new GuildTalentIdentityDTO(guildBonus));
            this.Aura = characterSource.Aura.Maybe(aura => new AuraIdentityDTO(aura));
            this.SharedAuras = characterSource.SharedAuras.ChangeKey(aura => new AuraIdentityDTO(aura));
            this.Buffs = characterSource.Buffs.ChangeKey(buff => new BuffIdentityDTO(buff));
            this.Talents = characterSource.Talents.ToList(t => new TalentIdentityDTO(t));
            this.CollectedItems = characterSource.CollectedItems.ToList(item => new CollectedItemIdentityDTO(item));

            this.EnableElixir = characterSource.EnableElixir;
            this.EnableConsumables = characterSource.EnableConsumables;
            this.EnableGuildTalents = characterSource.EnableGuildTalents;
            this.EnableAura = characterSource.EnableAura;
            this.EnableBuffs = characterSource.EnableBuffs;
            this.EnableTalents = characterSource.EnableTalents;
            this.LostControl = characterSource.LostControl;
            this.EnableCollecting = characterSource.EnableCollecting;
        }


        public ICharacterSource ToDomainObject(IMappingService mappingService)
        {
            if (mappingService == null) throw new ArgumentNullException(nameof(mappingService));

            return new CharacterSource
            {
                Class = this.Class.ToDomainObject(mappingService),

                Gender = this.Gender.ToDomainObject(mappingService),

                Level = this.Level,

                State = this.State.ToDomainObject(mappingService),

                Event = this.Event.ToDomainObject(mappingService),

                EditStats = this.EditStats.ChangeKey(v => v.ToDomainObject(mappingService)),

                Equipments = this.Equipments.ToDictionary(pair => pair.Key.ToDomainObject(mappingService), pair => pair.Value.ToDomainObject(mappingService)),

                Elixir = this.Elixir.ToDomainObject(mappingService),

                Consumables = this.Consumables.ToList(v => v.ToDomainObject(mappingService)),

                GuildTalents = this.GuildTalents.ChangeKey(v => v.ToDomainObject(mappingService)),

                Aura = this.Aura.ToDomainObject(mappingService),

                SharedAuras = this.SharedAuras.ChangeKey(v => v.ToDomainObject(mappingService)),

                Buffs = this.Buffs.ChangeKey(v => v.ToDomainObject(mappingService)),

                Talents = this.Talents.ToList(v => v.ToDomainObject(mappingService)),

                CollectedItems = this.CollectedItems.ToList(v => v.ToDomainObject(mappingService)),

                EnableElixir = this.EnableElixir,

                EnableConsumables = this.EnableConsumables,

                EnableGuildTalents = this.EnableGuildTalents,

                EnableAura = this.EnableAura,

                EnableBuffs = this.EnableBuffs,

                EnableTalents = this.EnableTalents,

                LostControl = this.LostControl,

                EnableCollecting = this.EnableCollecting
            };
        }
    }
}