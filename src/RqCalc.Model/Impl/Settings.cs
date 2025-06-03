using Framework.Core;
using RqCalc.Domain;

namespace RqCalc.Model.Impl;

public class Settings : ISettings
{
    public Settings(
        int weaponCardCount,
        int equipmentCardCount,
        int levelMultiplicity,
        int talentLevelMultiplicity,
        int statsPerLevel,
        int maxStatCount,
        int maxUpgradeLevel,
        string attackStatName,
        string defenseStatName,
        string hpStatName,
        int? customLastVersion,
        int qualityMaxLevel,
        DateTime updateDate)
    {
        if (weaponCardCount < 0) throw new ArgumentOutOfRangeException(nameof(weaponCardCount));
        if (equipmentCardCount < 0) throw new ArgumentOutOfRangeException(nameof(equipmentCardCount));
        if (levelMultiplicity <= 0) throw new ArgumentOutOfRangeException(nameof(levelMultiplicity));
        if (talentLevelMultiplicity <= 0) throw new ArgumentOutOfRangeException(nameof(talentLevelMultiplicity));
        if (statsPerLevel <= 0) throw new ArgumentOutOfRangeException(nameof(statsPerLevel));
        if (maxStatCount <= 0) throw new ArgumentOutOfRangeException(nameof(maxStatCount));
        if (qualityMaxLevel <= 0) throw new ArgumentOutOfRangeException(nameof(maxStatCount));


        if (attackStatName.IsNullOrWhiteSpace()) throw new ArgumentOutOfRangeException(nameof(attackStatName));
        if (defenseStatName.IsNullOrWhiteSpace()) throw new ArgumentOutOfRangeException(nameof(defenseStatName));
        if (hpStatName.IsNullOrWhiteSpace()) throw new ArgumentOutOfRangeException(nameof(hpStatName));

        this.WeaponCardCount = weaponCardCount;
        this.EquipmentCardCount = equipmentCardCount;
        this.LevelMultiplicity = levelMultiplicity;
        this.TalentLevelMultiplicity = talentLevelMultiplicity;
        this.StatsPerLevel = statsPerLevel;
        this.MaxStatCount = maxStatCount;
        this.MaxUpgradeLevel = maxUpgradeLevel;

        this.AttackStatName = attackStatName;
        this.DefenseStatName = defenseStatName;
        this.HpStatName = hpStatName;
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



    public string AttackStatName { get; }

    public string DefenseStatName { get; }

    public string HpStatName { get; }

    public int? CustomLastVersion { get; }

    public int QualityMaxLevel { get; }

    public DateTime UpdateDate { get; }


    public static Settings Create(IEnumerable<ISetting> settings)
    {
        if (settings == null) throw new ArgumentNullException(nameof(settings));

        var dict = settings.ToDictionary(s => s.Key, s => s.Value);

        return new Settings(
            int.Parse(dict["WeaponCardCount"]),
            int.Parse(dict["EquipmentCardCount"]),
            int.Parse(dict["LevelMultiplicity"]),
            int.Parse(dict["TalentLevelMultiplicity"]),
            int.Parse(dict["StatsPerLevel"]),
            int.Parse(dict["MaxStatCount"]),
            int.Parse(dict["MaxUpgradeLevel"]),
            dict["AttackStatName"],
            dict["DefenseStatName"],
            dict["HpStatName"],
            dict["CustomLastVersion"].Pipe(str => string.IsNullOrEmpty(str) ? default(int?) : int.Parse(str)),
            int.Parse(dict["QualityMaxLevel"]),
            DateTime.Parse(dict["UpdateDate"]));
    }
}