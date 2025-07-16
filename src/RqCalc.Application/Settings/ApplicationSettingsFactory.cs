using Framework.DataBase;

using RqCalc.Application._Extensions;
using RqCalc.Domain;
using RqCalc.Domain._Base;
using RqCalc.Model;

namespace RqCalc.Application.Settings;

public class ApplicationSettingsFactory(IDataSource<IPersistentDomainObjectBase> dataSource) : IApplicationSettingsFactory
{
    public ApplicationSettings Create()
    {
        var settingsDict = dataSource.GetFullList<ISetting>().ToDictionary(s => s.Key, s => s.Value);

        var stats = dataSource.GetFullList<IStat>().ToList();

        return new ApplicationSettings(
            int.Parse(settingsDict["WeaponCardCount"]),
            int.Parse(settingsDict["EquipmentCardCount"]),
            int.Parse(settingsDict["LevelMultiplicity"]),
            int.Parse(settingsDict["TalentLevelMultiplicity"]),
            int.Parse(settingsDict["StatsPerLevel"]),
            int.Parse(settingsDict["MaxStatCount"]),
            int.Parse(settingsDict["MaxUpgradeLevel"]),
            int.Parse(settingsDict["QualityMaxLevel"]),
            DateTime.Parse(settingsDict["UpdateDate"]),
            8,
            stats.GetByName(settingsDict["AttackStatName"]),
            stats.GetByName(settingsDict["DefenseStatName"]),
            stats.GetByName(settingsDict["HpStatName"]));
    }
}