using Framework.Core;
using Framework.DataBase;

using RqCalc.Domain._Base;
using RqCalc.Domain.Equipment;

namespace RqCalc.Application;

public class EquipmentForgeService(IDataSource<IPersistentDomainObjectBase> dataSource) : IEquipmentForgeService
{
    private readonly IEquipmentForge[] equipmentForges = dataSource.GetFullList<IEquipmentForge>().ToDictionary(v => v.Level).ToArrayI();

    private readonly IReadOnlyDictionary<Tuple<int, int>, int> equipmentLevelForges =

        dataSource.GetFullList<IEquipmentLevelForge>().ToDictionary(v => Tuple.Create(v.EquipmentLevel, v.Level), v => v.Hp);

    public IEquipmentForge GetEquipmentForge(int level) => this.equipmentForges[level];

    public int GetHpBonus(int equipmentLevel, int level) => this.equipmentLevelForges.GetValueOrDefault(Tuple.Create(equipmentLevel, level));
}