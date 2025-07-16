using Framework.Core;
using Framework.DataBase;
using RqCalc.Domain._Base;
using RqCalc.Domain.Equipment;
using RqCalc.Model;

namespace RqCalc.Application;

public class EquipmentService(ApplicationSettings settings, IDataSource<IPersistentDomainObjectBase> dataSource) : IEquipmentService
{
    private readonly IDictionaryCache<IEquipment, IEquipmentClass> equipmentClassCache = new DictionaryCache<IEquipment, IEquipmentClass>(equipment =>
    {
        var internalLevel = equipment.Info.Maybe(v => v.InternalLevel, 1);

        return dataSource.GetFullList<IEquipmentClass>().OrderByDescending(e => e.Level).First(c => internalLevel >= c.Level);

    }).WithLock();

    public IEquipmentClass GetEquipmentClass(IEquipment equipment) => this.equipmentClassCache[equipment];

    public int GetMaxUpgradeLevel(IEquipment equipment)
    {
        var @class = this.GetEquipmentClass(equipment);

        return @class.MaxUpgradeLevel ?? settings.MaxUpgradeLevel;
    }
}