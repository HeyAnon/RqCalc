namespace RqCalc.Domain.Model;

public interface ISettings
{
    int WeaponCardCount { get; }

    int EquipmentCardCount { get; }

    int MaxUpgradeLevel { get; }

    int StatsPerLevel { get; }

    int MaxStatCount { get; }

    int LevelMultiplicity { get; }

    int TalentLevelMultiplicity { get; }

    DateTime UpdateDate { get; }
}