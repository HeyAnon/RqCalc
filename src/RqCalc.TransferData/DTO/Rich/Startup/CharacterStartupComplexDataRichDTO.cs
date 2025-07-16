using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

using Framework.Core;

using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.TransferData
{
    [DataContract]
    public class CharacterStartupComplexDataRichDTO
    {
        [DataMember]
        public List<GenderSimpleDTO> Genders;
        
        [DataMember]
        public List<EventSimpleDTO> Events;

        [DataMember]
        public List<StateSimpleDTO> States;
        
        [DataMember]
        public List<StampColorSimpleDTO> StampColors;
        
        [DataMember]
        public List<EquipmentSlotRichDTO> Slots;

        [DataMember]
        public List<StatSimpleDTO> Stats;

        [DataMember]
        public List<EquipmentClassSimpleDTO> EquipmentClasses;

        [DataMember]
        public List<CardTypeSimpleDTO> CardTypes;

        [DataMember]
        public List<ElementSimpleDTO> Elements;

        [DataMember]
        public List<RaceSimpleDTO> Races;
        
        [DataMember]
        public List<BuffRichDTO> SharedBuffs;

        //----------------------------------------------------

        [DataMember]
        public List<EquipmentRichDTO> Equipments;

        [DataMember]
        public List<StampRichDTO> Stamps;

        [DataMember]
        public List<CardRichDTO> Cards;

        //----------------------------------------------------

        [DataMember]
        public List<ElixirRichDTO> Elixirs;

        [DataMember]
        public List<ConsumableRichDTO> Consumables;
        
        [DataMember]
        public List<EquipmentElixirRichDTO> EquipmentElixirs;

        [DataMember]
        public List<CollectedGroupRichDTO> CollectedGroups;

        [DataMember]
        public List<CollectedStatisticSimpleDTO> CollectedStatistics;

        [DataMember]
        public SettingsSimpleDTO Settings;

        [DataMember]
        public List<ClassRichDTO> Classes;

        [DataMember]
        public List<BonusTypeSimpleDTO> BonusTypes;

        [DataMember]
        public int SerializerVersion;

        [DataMember]
        public List<GuildTalentBranchRichDTO> GuildTalentBranches;


        [DataMember]
        public string DefaultCharacterCode;


        public CharacterStartupComplexDataRichDTO(IMappingService mappingService)
        {
            if (mappingService == null) throw new ArgumentNullException(nameof(mappingService));

            var context = mappingService.Context;
            
            this.Settings = context.Settings.Pipe(s => new SettingsSimpleDTO(s, mappingService));
            this.Genders = context.DataSource.GetFullList<IGender>().ToList(gender => new GenderSimpleDTO(gender, mappingService));
            
            this.Events = context.DataSource.GetFullList<IEvent>().WhereVersion(mappingService.Context.LastVersion).ToList(@event => new EventSimpleDTO(@event, mappingService));
            this.States = context.DataSource.GetFullList<IState>().ToList(state => new StateSimpleDTO(state, mappingService));
            this.StampColors = context.DataSource.GetFullList<IStampColor>().ToList(sc => new StampColorSimpleDTO(sc, mappingService));
            this.Slots = context.DataSource.GetFullList<IEquipmentSlot>().ToList(slot => new EquipmentSlotRichDTO(slot, mappingService));
            this.Stats = context.DataSource.GetFullList<IStat>().ToList(stat => new StatSimpleDTO(stat, mappingService));
            this.EquipmentClasses = context.DataSource.GetFullList<IEquipmentClass>().ToList(ec => new EquipmentClassSimpleDTO(ec, mappingService));
            this.CardTypes = context.DataSource.GetFullList<ICardType>().ToList(ct => new CardTypeSimpleDTO(ct, mappingService));
            this.Elements = context.DataSource.GetFullList<IElement>().ToList(element => new ElementSimpleDTO(element, mappingService));
            this.Races = context.DataSource.GetFullList<IRace>().ToList(race => new RaceSimpleDTO(race, mappingService));
            this.SharedBuffs = context.DataSource.GetFullList<IBuff>()
                                                 .WhereVersion(mappingService.Context.LastVersion)
                                                 .Where(buff => buff.IsShared())
                                                 .ToList(buff => new BuffRichDTO(buff, mappingService));

            this.Equipments = context.DataSource.GetFullList<IEquipment>()
                                                .WhereVersion(mappingService.Context.LastVersion)
                                                .ToList(e => new EquipmentRichDTO(e, mappingService));

            this.Stamps = context.DataSource.GetFullList<IStamp>().ToList(s => new StampRichDTO(s, mappingService));

            this.Cards = context.DataSource.GetFullList<ICard>()
                                           .WhereVersion(mappingService.Context.LastVersion)
                                           .ToList(c => new CardRichDTO(c, mappingService));

            this.Elixirs = context.DataSource.GetFullList<IElixir>().ToList(e => new ElixirRichDTO(e, mappingService));
            this.Consumables = context.DataSource.GetFullList<IConsumable>().ToList(c => new ConsumableRichDTO(c, mappingService));
            this.EquipmentElixirs = context.DataSource.GetFullList<IEquipmentElixir>().WhereVersion(mappingService.Context.LastVersion)
                                                                                      .ToList(ee => new EquipmentElixirRichDTO(ee, mappingService));

            this.CollectedGroups = context.DataSource.GetFullList<ICollectedGroup>().OrderBy(collectedGroup => collectedGroup.OrderIndex).ToList(g => new CollectedGroupRichDTO(g, mappingService));
            this.CollectedStatistics = context.DataSource.GetFullList<ICollectedStatistic>().OrderBy(collectedStatistic => collectedStatistic.OrderIndex).ToList(stat => new CollectedStatisticSimpleDTO(stat, mappingService));

            this.Settings = context.Settings.Pipe(s => new SettingsSimpleDTO(s, mappingService));
            this.Classes = context.DataSource.GetFullList<IClass>().ToList(@class => new ClassRichDTO(@class, mappingService));
            this.BonusTypes = context.DataSource.GetFullList<IBonusType>().ToList(bonusType => new BonusTypeSimpleDTO(bonusType, mappingService));

            this.GuildTalentBranches = context.DataSource.GetFullList<IGuildTalentBranch>().ToList(gb => new GuildTalentBranchRichDTO(gb, mappingService));

            this.DefaultCharacterCode = context.Calc(context.GetDefaultCharacter()).Code;
            this.SerializerVersion = context.LastVersion.Id;
        }
    }
}