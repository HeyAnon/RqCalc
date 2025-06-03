using System;
using System.Runtime.Serialization;



using Anon.RQ_Calc.Logic;

namespace Anon.RQ_Calc.TransferData
{
    [DataContract]
    public class SettingsSimpleDTO
    {
        [DataMember] 
        public int WeaponCardCount;

        [DataMember]
        public int EquipmentCardCount;

        [DataMember]
        public int StatsPerLevel;

        [DataMember]
        public int MaxStatCount;

        [DataMember]
        public int MaxUpgradeLevel;

        [DataMember] 
        public int LevelMultiplicity;

        [DataMember]
        public int TalentLevelMultiplicity;

        [DataMember]
        public StatIdentityDTO AttackStat;

        [DataMember]
        public StatIdentityDTO DefenseStat;

        [DataMember]
        public int MaxTalentLevel;

        public SettingsSimpleDTO(IApplicationSettings settings, IMappingService mappingService)
        {
            if (settings == null) throw new ArgumentNullException(nameof(settings));
            if (mappingService == null) throw new ArgumentNullException(nameof(mappingService));

            this.WeaponCardCount = settings.WeaponCardCount;
            this.EquipmentCardCount = settings.EquipmentCardCount;
            this.StatsPerLevel = settings.StatsPerLevel;
            this.MaxStatCount = settings.MaxStatCount;
            this.MaxUpgradeLevel = settings.MaxUpgradeLevel;
            this.LevelMultiplicity = settings.LevelMultiplicity;
            this.TalentLevelMultiplicity = settings.TalentLevelMultiplicity;
            this.MaxTalentLevel = mappingService.Context.LastVersion.MaxTalentLevel;

            this.AttackStat = new StatIdentityDTO(settings.AttackStat);
            this.DefenseStat = new StatIdentityDTO(settings.DefenseStat);
        }
    }
}