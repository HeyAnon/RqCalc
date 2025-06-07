namespace RqCalc.Model.Impl;

public record Settings(
    int WeaponCardCount,
    int EquipmentCardCount,
    int LevelMultiplicity,
    int TalentLevelMultiplicity,
    int StatsPerLevel,
    int MaxStatCount,
    int MaxUpgradeLevel,
    string AttackStatName,
    string DefenseStatName,
    string HpStatName,
    int QualityMaxLevel,
    DateTime UpdateDate)
    : ISettings;

//public static Settings Create(IEnumerable<ISetting> settings)
//{
//    if (settings == null) throw new ArgumentNullException(nameof(settings));

//    var dict = settings.ToDictionary(s => s.Key, s => s.Value);

//    return new Settings(
//        int.Parse(dict["WeaponCardCount"]),
//        int.Parse(dict["EquipmentCardCount"]),
//        int.Parse(dict["LevelMultiplicity"]),
//        int.Parse(dict["TalentLevelMultiplicity"]),
//        int.Parse(dict["StatsPerLevel"]),
//        int.Parse(dict["MaxStatCount"]),
//        int.Parse(dict["MaxUpgradeLevel"]),
//        dict["AttackStatName"],
//        dict["DefenseStatName"],
//        dict["HpStatName"],
//        dict["CustomLastVersion"].Pipe(str => string.IsNullOrEmpty(str) ? default(int?) : int.Parse(str)),
//        int.Parse(dict["QualityMaxLevel"]),
//        DateTime.Parse(dict["UpdateDate"]));
//}