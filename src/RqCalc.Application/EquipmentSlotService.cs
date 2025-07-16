using Framework.DataBase;

using RqCalc.Domain._Base;
using RqCalc.Domain._Extensions;
using RqCalc.Domain.Equipment;
using RqCalc.Model;

namespace RqCalc.Application;

public class EquipmentSlotService(ApplicationSettings settings, IDataSource<IPersistentDomainObjectBase> dataSource) : IEquipmentSlotService
{
    public IEquipmentSlot PrimaryWeaponSlot { get; } = dataSource.GetFullList<IEquipmentSlot>().Single(s => s.IsPrimarySlot());

    public int GetMaxCardCount(IEquipmentSlot slot) =>
        slot.IsWeapon switch
        {
            true => settings.WeaponCardCount,
            false => settings.EquipmentCardCount,
            _ => 0
        };
}