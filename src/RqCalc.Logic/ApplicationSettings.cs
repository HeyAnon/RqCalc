using RqCalc.Domain;
using RqCalc.Model;

namespace RqCalc.Logic;

public class ApplicationSettings : IApplicationSettings
{
    public ApplicationSettings(
        int weaponCardCount,
        int equipmentCardCount,
        int levelMultiplicity,
        int talentLevelMultiplicity,
        int statsPerLevel,
        int maxStatCount,
        int maxUpgradeLevel,
        IStat attackStat,
        IStat defenseStat,
        IStat hpStat,
        IVersion customLastVersion,
        int qualityMaxLevel,
        DateTime updateDate)
    {
        if (weaponCardCount < 0) throw new ArgumentOutOfRangeException(nameof(weaponCardCount));
        if (equipmentCardCount < 0) throw new ArgumentOutOfRangeException(nameof(equipmentCardCount));
        if (levelMultiplicity <= 0) throw new ArgumentOutOfRangeException(nameof(levelMultiplicity));
        if (talentLevelMultiplicity <= 0) throw new ArgumentOutOfRangeException(nameof(talentLevelMultiplicity));
        if (statsPerLevel <= 0) throw new ArgumentOutOfRangeException(nameof(statsPerLevel));
        if (maxStatCount <= 0) throw new ArgumentOutOfRangeException(nameof(maxStatCount));
        if (qualityMaxLevel <= 0) throw new ArgumentOutOfRangeException(nameof(qualityMaxLevel));

        this.WeaponCardCount = weaponCardCount;
        this.EquipmentCardCount = equipmentCardCount;
        this.LevelMultiplicity = levelMultiplicity;
        this.TalentLevelMultiplicity = talentLevelMultiplicity;
        this.StatsPerLevel = statsPerLevel;
        this.MaxStatCount = maxStatCount;
        this.MaxUpgradeLevel = maxUpgradeLevel;

        this.AttackStat = attackStat ?? throw new ArgumentNullException(nameof(attackStat));
        this.DefenseStat = defenseStat ?? throw new ArgumentNullException(nameof(defenseStat));
        this.HpStat = hpStat ?? throw new ArgumentNullException(nameof(hpStat));
        this.CustomLastVersion = customLastVersion;
        this.QualityMaxLevel = qualityMaxLevel;
        this.UpdateDate = updateDate;
    }

    public int WeaponCardCount { get; }

    public int EquipmentCardCount { get; }

    public int LevelMultiplicity { get; }

    public int TalentLevelMultiplicity { get; }

    public int StatsPerLevel { get; }

    public int MaxStatCount { get; }

    public int MaxUpgradeLevel { get; }

    public int QualityMaxLevel { get; }

    public DateTime UpdateDate { get; }

    public IStat AttackStat { get; }

    public IStat DefenseStat { get; }

    /// <summary>
    /// Move to db settings
    /// </summary>
    public int MaxPartySize { get; } = 8;

    public IStat HpStat { get; }

    public IVersion? CustomLastVersion { get; }
}

public interface IApplicationSettings : ISettings
{
    IStat AttackStat { get; }

    IStat DefenseStat { get; }


    int MaxPartySize { get; }
}