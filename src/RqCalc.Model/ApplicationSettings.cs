using RqCalc.Domain;

namespace RqCalc.Model;

public record ApplicationSettings(
    int WeaponCardCount,
    int EquipmentCardCount,
    int LevelMultiplicity,
    int TalentLevelMultiplicity,
    int StatsPerLevel,
    int MaxStatCount,
    int MaxUpgradeLevel,
    int QualityMaxLevel,
    DateTime UpdateDate,
    int MaxPartySize,
    IStat AttackStat,
    IStat DefenseStat,
    IStat HpStat);
