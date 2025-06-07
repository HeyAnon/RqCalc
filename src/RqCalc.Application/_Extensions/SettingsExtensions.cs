using Framework.Core;
using Framework.DataBase;
using RqCalc.Domain;
using RqCalc.Domain._Base;
using RqCalc.Domain.Equipment;
using RqCalc.Model;
using RqCalc.Model.Impl;

namespace RqCalc.Application._Extensions;

public static class SettingsExtensions
{
    public static ApplicationSettings ToApplicationSettings(this Settings dbSettings, IDataSource<IPersistentDomainObjectBase> dataSource)
    {
        if (dbSettings == null) throw new ArgumentNullException(nameof(dbSettings));
        if (dataSource == null) throw new ArgumentNullException(nameof(dataSource));

        var stats = dataSource.GetFullList<IStat>().ToList();

        return new ApplicationSettings(
            dbSettings.WeaponCardCount,
            dbSettings.EquipmentCardCount,
            dbSettings.LevelMultiplicity,
            dbSettings.TalentLevelMultiplicity,
            dbSettings.StatsPerLevel,
            dbSettings.MaxStatCount,
            dbSettings.MaxUpgradeLevel,
            stats.GetByName(dbSettings.AttackStatName),
            stats.GetByName(dbSettings.DefenseStatName),
            stats.GetByName(dbSettings.HpStatName),
            dbSettings.CustomLastVersion.MaybeNullable(version => dataSource.GetFullList<IVersion>().GetById(version)),
            dbSettings.QualityMaxLevel,
            dbSettings.UpdateDate);
    }

    public static int GetMaxCardCount(this ISettings settings, IEquipmentSlot slot)
    {
        if (settings == null) throw new ArgumentNullException(nameof(settings));
        if (slot == null) throw new ArgumentNullException(nameof(slot));

        switch (slot.IsWeapon)
        {
            case true:
                return settings.WeaponCardCount;

            case false:
                return settings.EquipmentCardCount;

            default:
                return 0;
        }
    }
}